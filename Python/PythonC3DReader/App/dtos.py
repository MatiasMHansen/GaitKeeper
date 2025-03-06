from pydantic import BaseModel
from typing import Any, List, Optional

class Biometrics(BaseModel):
    Height: Any
    Weight: Any
    LLegLength: Any
    RLegLength: Any

class SystemInfo(BaseModel):
    Software: Any
    Version: Any
    MarkerSetup: Any

class GaitSession(BaseModel): # Aggregate
    SubjectId: Any
    Biometrics: Biometrics
    SystemInfo: SystemInfo

class Unit(BaseModel):
    X : Any
    Y : Any
    Z : Any

class Marker(BaseModel):
    Label: Any
    UnitType: Any
    Units: List[Unit]

class GaitEvents(BaseModel):
    Label: Any
    Context: Any
    Description: Any
    Time: Any

class PointData(BaseModel): # Aggregate
    StartFrame: Any
    EndFrame: Any
    PointFreq: Any
    AllLabels: Any
    Markers: List[Marker]
    GaitEvents: GaitEvents
