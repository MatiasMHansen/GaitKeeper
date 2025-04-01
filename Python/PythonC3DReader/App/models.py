from pydantic import BaseModel
from typing import Any, List, Optional, Type

class Biometrics(BaseModel):
    Height: float
    Weight: float
    LLegLength: float
    RLegLength: float

class SystemInfo(BaseModel):
    Software: str
    Version: str
    MarkerSetup: str

class GaitAnalysis(BaseModel):
    Name: str
    Description: str
    Context: str
    UnitType: str
    Value: float

class GaitCycle(BaseModel):
    Name: str
    Description: str
    Number: int
    StartFrame: int
    EndFrame: int
    EventStart: float
    EventEnd: float
    Duration: float

class GaitSession(BaseModel): # Aggregate
    FileName: str
    SubjectId: str
    PointFreq: float
    AnalogFreq: float
    StartFrame: int
    EndFrame: int
    TotalFrames: int
    AngleLabels: List[str]
    ForceLabels: List[str]
    ModeledLabels: List[str]
    MomentLabels: List[str]
    PowerLabels: List[str]
    PointLabels: List[str]
    Biometrics: Biometrics
    SystemInfo: SystemInfo
    LGaitCycles: List[GaitCycle]
    RGaitCycles: List[GaitCycle]
    GaitAnalyses: Optional[List[GaitAnalysis]] = None

class Unit(BaseModel):
    X: Optional[float]
    Y: Optional[float]
    Z: Optional[float]

class Marker(BaseModel):
    Label: str
    UnitType: str
    Units: List[Unit]

class PointData(BaseModel): # Aggregate
    FileName: str
    SubjectId: str
    PointFreq: float
    StartFrame: int
    EndFrame: int
    TotalFrames: int
    PointMarkers: List[Marker]
    AngleMarkers: List[Marker]
    ForceMarkers: List[Marker]
    ModeledMarkers: List[Marker]
    MomentMarkers: List[Marker]
    PowerMarkers: List[Marker]
    LGaitCycles: List[GaitCycle]
    RGaitCycles: List[GaitCycle]
