import ezc3d
import os
import numpy as np
from typing import List, Dict
from App.interfaces import IC3DParserService
from config import Config
from App.dtos import Biometrics, GaitEvents, SystemInfo, GaitSession, Unit, Marker, PointData

class C3DParserService(IC3DParserService):
    # Intern metode til at indlæse C3D-filen
    def _load_c3d_file(self, file_name: str):
        file_path = os.path.join(Config.DOWNLOADS_FOLDER, file_name)
        if not os.path.exists(file_path):
            raise FileNotFoundError(f"C3D-filen '{file_name}' blev ikke fundet i {Config.DOWNLOADS_FOLDER}")
        return ezc3d.c3d(file_path)
    
    # Helper funktion til robust parameterhåndtering (håndterer NumPy-datatyper).
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

    # Metode til at hente metadata fra C3D-filen
    def get_gait_session(self, file_name: str) -> dict:
        # Load & setup
        c3d = self._load_c3d_file(file_name)
        parameters = c3d['parameters']

        # Extract GaitSession
        gait_session = GaitSession(
            SubjectId=self._fetch_value(parameters, 'SUBJECTS', 'NAMES'),
            
            # Extract Biometrics
            Biometrics=Biometrics(
                Height=self._fetch_value(parameters, 'PROCESSING', 'Height'),
                Weight=self._fetch_value(parameters, 'PROCESSING', 'Bodymass'),
                LLegLength=self._fetch_value(parameters, 'PROCESSING', 'LLegLength'),
                RLegLength=self._fetch_value(parameters, 'PROCESSING', 'RLegLength')
                # Add more biometrics here
            ),

            # Extract System Info
            SystemInfo=SystemInfo(
                Software=self._fetch_value(parameters, 'MANUFACTURER', 'SOFTWARE'),
                Version=self._fetch_value(parameters, 'MANUFACTURER', 'VERSION_LABEL'),
                MarkerSetup=self._fetch_value(parameters, 'SUBJECTS', 'MARKER_SETS')
            )
        )

        # Returnér som dict
        return gait_session.model_dump()
        
    # Metode til at hente punktdata fra C3D-filen
    def get_point_data(self, file_name: str) -> dict:
        # Load & setup
        c3d = self._load_c3d_file(file_name)
        parameters = c3d['parameters']
        point_data = c3d["data"]["points"]

        # Extract PointData
        point_freq = self._fetch_value(parameters, 'POINT', 'RATE')
        start_frame = self._fetch_value(parameters, 'TRIAL', 'ACTUAL_START_FIELD')
        end_frame = self._fetch_value(parameters, 'TRIAL', 'ACTUAL_END_FIELD')
        all_labels = self._fetch_list(parameters, 'POINT', 'LABELS')

        # Extract GaitEvents
        gait_events = GaitEvents(
            Label = self._fetch_list(parameters, 'EVENT', 'LABELS'),
            Context = self._fetch_list(parameters, 'EVENT', 'CONTEXTS'),
            Description = self._fetch_list(parameters, 'EVENT', 'DESCRIPTIONS'),
            Time = self._fetch_list(parameters, 'EVENT', 'TIMES')
        )

        # Debugging: Matcher antal labels og markører
        total_frames = end_frame - start_frame + 1
        if len(all_labels) == point_data.shape[1]:
            print(f"Succes! - Antal labels ({len(all_labels)}) matcher antal markører ({point_data.shape[1]})")
        else:
            print(f"Warning! - Antal labels ({len(all_labels)}) matcher ikke antal markører ({point_data.shape[1]})")
        
        # Debugging: Frames
        if total_frames == point_data.shape[2]:
            print(f"Succes! - Antallet af frames ({total_frames}) matcher antallet af målinger ({point_data.shape[2]})")
        else:
            print(f"Warning! - Antallet af frames ({total_frames}) matcher ikke antallet af målinger ({point_data.shape[2]})")
        
        # Hent labels, som lister / arrays
        angle_labels = self._fetch_list(parameters, 'POINT', 'ANGLES')
        force_labels = self._fetch_list(parameters, 'POINT', 'FORCES')
        modeled_labels = self._fetch_list(parameters, 'POINT', 'MODELED_MARKERS')
        moment_labels = self._fetch_list(parameters, 'POINT', 'MOMENTS')
        power_labels = self._fetch_list(parameters, 'POINT', 'POWERS')
        
        # Hent måleenheder for markørerne
        angle_unit = self._fetch_value(parameters, 'POINT', 'ANGLE_UNITS')
        force_unit = self._fetch_value(parameters, 'POINT', 'FORCE_UNITS')
        modeled_unit = self._fetch_value(parameters, 'POINT', 'MODELED_MARKER_UNITS')
        moment_unit = self._fetch_value(parameters, 'POINT', 'MOMENT_UNITS')
        power_unit = self._fetch_value(parameters, 'POINT', 'POWER_UNITS')
        
        # Placeholder: Liste over alle markører
        markers_list = []

        # Loop gennem markører
        for index in range(point_data.shape[1]):  # nb_markers
            label = all_labels[index] if index < len(all_labels) else f"Marker_{index}"
            units_list = []
            
            # UnitType baseret på hvilken liste 'label' findes i
            if label in angle_labels:
                unit_type = angle_unit
            elif label in force_labels:
                unit_type = force_unit
            elif label in modeled_labels:
                unit_type = modeled_unit
            elif label in moment_labels:
                unit_type = moment_unit
            elif label in power_labels:
                unit_type = power_unit
            else:
                unit_type = self._fetch_value(parameters, 'POINT', 'UNITS')

            # Loop gennem frames
            for frame in range(point_data.shape[2]):  # nb_frames
                x, y, z, _ = point_data[:, index, frame]  # Sidste værdi (_) er residual/error

                # Tilføj frame data til units_list
                units_list.append(Unit(
                    X=float(x),
                    Y=float(y),
                    Z=float(z)
                ))

            # Opret marker objekt
            markers_list.append(Marker(
                Label=label,
                UnitType=unit_type,
                Units=units_list
            ))

        # Opret PointData objekt
        data = PointData(
            StartFrame=start_frame,
            EndFrame=end_frame,
            PointFreq=point_freq,
            AllLabels=all_labels,
            Markers=markers_list,
            GaitEvents=gait_events
        )

        # Returnér som dict
        return data.model_dump()