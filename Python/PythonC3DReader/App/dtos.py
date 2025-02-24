from pydantic import BaseModel
from datetime import date

class MetadataDTO(BaseModel):
    SubjectId: str
    Date: date
    TotalFrames: int
    Manufacture: str




