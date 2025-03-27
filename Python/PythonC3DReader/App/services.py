from optparse import Values
from pydoc import describe
from tkinter.font import names
import ezc3d
import os
import math
import numpy as np
from typing import List, Dict
from App.interfaces import IC3DParserService
from config import Config
from App.models import Biometrics, GaitCycle, SystemInfo, GaitSession, Unit, Marker, PointData, GaitAnalysis

class C3DParserService(IC3DParserService):
    # Intern metode til at indlæse C3D-filen
    def _load_c3d_file(self, file_name: str):
        file_path = os.path.join(Config.DOWNLOADS_FOLDER, file_name)
        if not os.path.exists(file_path):
            raise FileNotFoundError(f"C3D-filen '{file_name}' blev ikke fundet i {Config.DOWNLOADS_FOLDER}")
        return ezc3d.c3d(file_path)
    
    # Intern hjælpe funktioner til robust parameterhåndtering (håndterer NumPy-datatyper).
    def _fetch_value(self, parameters: dict, group: str, variable: str, default=None):
        value = parameters.get(group, {}).get(variable, {}).get('value', [default])[0]

        # Hvis værdien er en NumPy type, konverteres den til standard Python-typer
        if isinstance(value, (np.int64, np.int32)):  
            return int(value) 
        elif isinstance(value, (np.float64, np.float32)):  
            return float(value)

        return value

    def _fetch_list(self, parameters: dict, group: str, variable: str, default=None):
        value_list = parameters.get(group, {}).get(variable, {}).get('value', default or [])

        # Hvis value_list er en NumPy array, konverter det til en Python liste
        if isinstance(value_list, np.ndarray):
            value_list = value_list.tolist()

        # Hvis value_list indeholder nested NumPy arrays, konverter alle elementer
        return [
            v.tolist() if isinstance(v, np.ndarray) else
            int(v) if isinstance(v, (np.int64, np.int32)) else
            float(v) if isinstance(v, (np.float64, np.float32)) else v
            for v in value_list
        ]

    # Intern metode til at omstrukturere GaitEvents til GaitCycles
    def _create_gait_cycle_list(self, parameters: dict, context: str) -> List[GaitCycle]:
        # setup
        freq = self._fetch_value(parameters, 'POINT', 'RATE')

        event_labels = self._fetch_list(parameters, 'EVENT', 'LABELS')
        event_contexts = self._fetch_list(parameters, 'EVENT', 'CONTEXTS')
        event_descriptions = self._fetch_list(parameters, 'EVENT', 'DESCRIPTIONS')
        event_times = self._fetch_list(parameters, 'EVENT', 'TIMES')  # [1] = tider i sekunder

        if not isinstance(event_times, list) or len(event_times) < 2:
            return []  # sikkerhed

        time_values = event_times[1]  # tid i sekunder

        # Trin 1: Filtrer Foot Strike events
        foot_strike_events = []
        for i in range(len(event_labels)):
            if event_labels[i] == "Foot Strike" and event_contexts[i] == context:
                foot_strike_events.append({
                    "label": event_labels[i],
                    "context": event_contexts[i],
                    "description": event_descriptions[i],
                    "time": time_values[i]
                })

        # Trin 2: Opret GaitCycle-objekter parvis
        gait_cycles = []
        for i in range(len(foot_strike_events) - 1):
            start_event = foot_strike_events[i]
            end_event = foot_strike_events[i + 1]

            # Beregn frames
            start_frame = round(start_event["time"] * freq)
            end_frame = round(end_event["time"] * freq)
            duration = round(end_event["time"] - start_event["time"], 3)

            # GaitCycle DTO
            gait_cycles.append(GaitCycle(
                Name=f"{start_event['context']} {start_event['label']}",
                Description=start_event["description"],
                Number=i + 1,
                StartFrame=start_frame,
                EndFrame=end_frame,
                EventStart=start_event["time"],
                EventEnd=end_event["time"],
                Duration=duration
            ))

        return gait_cycles

    # Metode til at hente metadata fra C3D-filen
    def get_gait_session(self, file_name: str) -> dict:
        # Load & setup
        c3d = self._load_c3d_file(file_name)
        parameters = c3d['parameters']

        # Check GaitAnalysis
        analysis_exists = self._fetch_list(parameters, 'ANALYSIS', 'USED')
        gait_analysis_list = None

        if analysis_exists[0] > 0:
            names = self._fetch_list(parameters, 'ANALYSIS', 'NAMES')
            descriptions = self._fetch_list(parameters, 'ANALYSIS', 'DESCRIPTIONS')
            contexts = self._fetch_list(parameters, 'ANALYSIS', 'CONTEXTS')
            units = self._fetch_list(parameters, 'ANALYSIS', 'UNITS')
            values = self._fetch_list(parameters, 'ANALYSIS', 'VALUES')

            gait_analysis_list = [
                GaitAnalysis(
                    Name=names[i],
                    Description=descriptions[i],
                    Context=contexts[i],
                    UnitType=units[i],
                    Value=values[i]
                )
                for i in range(len(names))
            ]

        # Saml GaitSession
        gait_session = GaitSession(
            FileName=file_name,
            SubjectId=self._fetch_value(parameters, 'SUBJECTS', 'NAMES'),
            PointFreq=self._fetch_value(parameters, 'POINT', 'RATE'),
            AnalogFreq=self._fetch_value(parameters, 'ANALOG', 'RATE'),
            StartFrame=self._fetch_value(parameters, 'TRIAL', 'ACTUAL_START_FIELD'),
            EndFrame=self._fetch_value(parameters, 'TRIAL', 'ACTUAL_END_FIELD'),
            TotalFrames=self._fetch_value(parameters, 'TRIAL', 'ACTUAL_END_FIELD') - self._fetch_value(parameters, 'TRIAL', 'ACTUAL_START_FIELD') + 1,

            Biometrics=Biometrics(
                Height=self._fetch_value(parameters, 'PROCESSING', 'Height'),
                Weight=self._fetch_value(parameters, 'PROCESSING', 'Bodymass'),
                LLegLength=self._fetch_value(parameters, 'PROCESSING', 'LLegLength'),
                RLegLength=self._fetch_value(parameters, 'PROCESSING', 'RLegLength')
            ),

            SystemInfo=SystemInfo(
                Software=self._fetch_value(parameters, 'MANUFACTURER', 'SOFTWARE'),
                Version=self._fetch_value(parameters, 'MANUFACTURER', 'VERSION_LABEL'),
                MarkerSetup=self._fetch_value(parameters, 'SUBJECTS', 'MARKER_SETS')
            ),

            GaitAnalyses=gait_analysis_list,
            LGaitCycles=self._create_gait_cycle_list(parameters, context="Left"),
            RGaitCycles=self._create_gait_cycle_list(parameters, context="Right")
        )
        # Return som dict
        return gait_session.model_dump()
        
    # Metode til at hente punktdata fra C3D-filen
    def get_point_data(self, file_name: str) -> dict:
        # Load & setup
        c3d = self._load_c3d_file(file_name)
        parameters = c3d['parameters']
        point_data = c3d["data"]["points"]

        # Extract PointData
        subject_id=self._fetch_value(parameters, 'SUBJECTS', 'NAMES')
        point_freq = self._fetch_value(parameters, 'POINT', 'RATE')
        start_frame = self._fetch_value(parameters, 'TRIAL', 'ACTUAL_START_FIELD')
        end_frame = self._fetch_value(parameters, 'TRIAL', 'ACTUAL_END_FIELD')
        total_frames = end_frame - start_frame + 1
        all_labels = self._fetch_list(parameters, 'POINT', 'LABELS')

        # Extract GaitCycles
        right_gait_cycles = self._create_gait_cycle_list(parameters, context="Right")
        left_gait_cycles = self._create_gait_cycle_list(parameters, context="Left")

        # Setup for markørgrupper
        # Labels i grupper
        label_groups = {
            "AngleMarkers": self._fetch_list(parameters, 'POINT', 'ANGLES'),
            "ForceMarkers": self._fetch_list(parameters, 'POINT', 'FORCES'),
            "ModeledMarkers": self._fetch_list(parameters, 'POINT', 'MODELED_MARKERS'),
            "MomentMarkers": self._fetch_list(parameters, 'POINT', 'MOMENTS'),
            "PowerMarkers": self._fetch_list(parameters, 'POINT', 'POWERS'),
        }

        # Enheder
        unit_lookup = {
            "AngleMarkers": self._fetch_value(parameters, 'POINT', 'ANGLE_UNITS'),
            "ForceMarkers": self._fetch_value(parameters, 'POINT', 'FORCE_UNITS'),
            "ModeledMarkers": self._fetch_value(parameters, 'POINT', 'MODELED_MARKER_UNITS'),
            "MomentMarkers": self._fetch_value(parameters, 'POINT', 'MOMENT_UNITS'),
            "PowerMarkers": self._fetch_value(parameters, 'POINT', 'POWER_UNITS'),
            "PointMarkers": self._fetch_value(parameters, 'POINT', 'UNITS'),
        }

        # Initialiser lister for kategorier
        categorized_markers = {
            "PointMarkers": [],
            "AngleMarkers": [],
            "ForceMarkers": [],
            "ModeledMarkers": [],
            "MomentMarkers": [],
            "PowerMarkers": []
        }

        # Loop gennem alle markører
        for i in range(point_data.shape[1]):
            label = all_labels[i] if i < len(all_labels) else f"Marker_{i}"

            # Find hvilken kategori label tilhører
            category = "PointMarkers"  # default
            for group, labels in label_groups.items():
                if label in labels:
                    category = group
                    break

            # Enhedstype for denne markør
            unit_type = unit_lookup[category]

            # Saml frame data
            units_list = []
            for frame in range(point_data.shape[2]):
                x, y, z, _ = point_data[:, i, frame]
                units_list.append(Unit(
                    X=None if math.isnan(x) else float(x),
                    Y=None if math.isnan(y) else float(y),
                    Z=None if math.isnan(z) else float(z)
                ))

            # Tilføj til korrekt liste
            categorized_markers[category].append(Marker(
                Label=label,
                UnitType=unit_type,
                Units=units_list
            ))

        # Saml PointData objekt
        data = PointData(
            FileName=file_name,
            SubjectId=subject_id,
            PointFreq=point_freq,
            StartFrame=start_frame,
            EndFrame=end_frame,
            TotalFrames=total_frames,
            PointMarkers=categorized_markers["PointMarkers"],
            AngleMarkers=categorized_markers["AngleMarkers"],
            ForceMarkers=categorized_markers["ForceMarkers"],
            ModeledMarkers=categorized_markers["ModeledMarkers"],
            MomentMarkers=categorized_markers["MomentMarkers"],
            PowerMarkers=categorized_markers["PowerMarkers"],
            RGaitCycles=right_gait_cycles,
            LGaitCycles=left_gait_cycles
        )

        # debugging:
        print(f"SubjectId: {data.SubjectId}, FileName: {data.FileName}")
        print(f"StartFrame: {data.StartFrame}, EndFrame: {data.EndFrame}")
        print(f"TotalFrames: {data.TotalFrames}")
        print(f"Frequency: {data.PointFreq}")
        print(f"Antal PointMarkers: {len(data.PointMarkers)}")
        print(f"PointMarkers unittype: {data.PointMarkers[0].UnitType}")
        print(f"Antal AngleMarkers: {len(data.AngleMarkers)}")
        print(f"AngleMarkers unittype: {data.AngleMarkers[0].UnitType}")
        print(f"Antal ForceMarkers: {len(data.ForceMarkers)}")
        print(f"ForceMarkers unittype: {data.ForceMarkers[0].UnitType}")
        print(f"Antal ModeledMarkers: {len(data.ModeledMarkers)}")
        print(f"ModeledMarkers unittype: {data.ModeledMarkers[0].UnitType}")
        print(f"Antal MomentMarkers: {len(data.MomentMarkers)}")
        print(f"MomentMarkers unittype: {data.MomentMarkers[0].UnitType}")
        print(f"Antal PowerMarkers: {len(data.PowerMarkers)}")
        print(f"PowerMarkers unittype: {data.PowerMarkers[0].UnitType}")
        print(f"Det totale antal markører: {len(all_labels)}")

        # Return som dict
        return data.model_dump()