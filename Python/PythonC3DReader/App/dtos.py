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
    X: Optional[float]
    Y: Optional[float]
    Z: Optional[float]

class Marker(BaseModel):
    Label: str
    UnitType: str
    Units: List[Unit]

class GaitEvents(BaseModel):
    Label: List[str]
    Context: List[str]
    Description: List[str]
    Time: List[float]

class PointData(BaseModel): # Aggregate
    StartFrame: int
    EndFrame: int
    PointFreq: float
    AllLabels: List[str]
    Markers: List[Marker]
    GaitEvents: GaitEvents
