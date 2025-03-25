from pydantic import BaseModel
from typing import Any, List, Optional, Type

class Biometrics(BaseModel):
    Height: Any
    Weight: Any
    LLegLength: Any
    RLegLength: Any

class SystemInfo(BaseModel):
    Software: Any
    Version: Any
    MarkerSetup: Any

class GaitAnalysis(BaseModel):
    Name: Any
    Description: Any
    Context: Any
    UnitType: Any
    Value: Any

class GaitCycle(BaseModel):
    Name: Any
    Description: Any
    Number: Any
    StartFrame: Any
    EndFrame: Any
    EventStart: Any
    EventEnd: Any
    Duration: Any

class GaitSession(BaseModel): # Aggregate
    FileName: Any
    SubjectId: Any
    PointFreq: Any
    AnalogFreq: Any
    StartFrame: Any
    EndFrame: Any
    TotalFrames: Any
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
    Label: Any
    UnitType: Any
    Units: List[Unit]

class PointData(BaseModel): # Aggregate
    FileName: Any
    SubjectId: Any
    PointFreq: Any
    StartFrame: Any
    EndFrame: Any
    TotalFrames: Any
    PointMarkers: List[Marker]
    AngleMarkers: List[Marker]
    ForceMarkers: List[Marker]
    ModeledMarkers: List[Marker]
    MomentMarkers: List[Marker]
    PowerMarkers: List[Marker]
    LGaitCycles: List[GaitCycle]
    RGaitCycles: List[GaitCycle]
