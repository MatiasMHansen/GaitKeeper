import ezc3d
import os
import numpy as np
from typing import List, Dict
from App.interfaces import IC3DParserService
from App.dtos import MetadataDTO
from datetime import date
from config import Config

class C3DParserService(IC3DParserService):
    # Intern metode til at indlæse C3D-filen (måske ikke relevant)
    def _load_c3d_file(self, file_name: str):
        file_path = os.path.join(Config.DOWNLOADS_FOLDER, file_name)
        if not os.path.exists(file_path):
            raise FileNotFoundError(f"C3D-filen '{file_name}' blev ikke fundet i {Config.DOWNLOADS_FOLDER}")
        return ezc3d.c3d(file_path)
        
    # Metode til at hente metadata fra C3D-filen
    def get_metadata(self, file_name: str) -> dict:
        c3d = self._load_c3d_file(file_name)
        parameters = c3d["parameters"]

        # Ekstraherer ALLE parametre dynamisk
        all_parameters = {}

        for group_name, group in parameters.items():
            all_parameters[group_name] = {}

            for param_name, param in group.items():
                # Sikrer at 'param' har en 'value'-nøgle, ellers spring over
                if "value" not in param or param["value"] is None:
                    all_parameters[group_name][param_name] = None
                    continue
            
                value = param["value"]

                # Konverter numpy-typer til standard Python-typer
                if isinstance(value, np.ndarray):
                    value = value.tolist()  # Konverter numpy array til liste
                elif isinstance(value, (np.int64, np.float64)):
                    value = value.item()  # Konverter numpy int/float til Python int/float
        
                all_parameters[group_name][param_name] = value

        # Returnerer samlet metadata-struktur
        return all_parameters
        
    # Metode til at hente punktdata fra C3D-filen
    def get_point_data(self, file_name: str) -> List[Dict]:
        # Ekstraher punktdata fra en C3D-fil.
        c3d = self._load_c3d_file(file_name)
        
        # Hent point-data array (form: (4, nbMarkers, nbFrames))
        point_data = c3d["data"]["points"]

        # Antal frames og markører
        nb_frames = point_data.shape[2]  # Frames ligger langs tredje dimension
        nb_markers = point_data.shape[1]  # Markører ligger langs anden dimension

        frames_list = []

        for frame in range(nb_frames):
            frame_data = {
                "frame": frame,
                "points": []
            }
            
            for marker_id in range(nb_markers):
                x, y, z, _ = point_data[:, marker_id, frame]  # Sidste værdi (_) er residual/error
                
                frame_data["points"].append({
                    "id": marker_id,
                    "x": float(x),
                    "y": float(y),
                    "z": float(z)
                })

            frames_list.append(frame_data)

        return frames_list