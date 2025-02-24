from App.interfaces import IC3DParserService
from App.dtos import MetadataDTO
from datetime import date

class C3DParserService(IC3DParserService):
    # Intern metode til at indlæse C3D-filen
    def _load_c3d_file(self, file_name: str):
        raise NotImplementedError("Method _load_c3d_file() is not yet implemented")
        
    # Metode til at hente metadata fra C3D-filen
    def get_metadata(self, file_name: str) -> MetadataDTO:
        return MetadataDTO(
            SubjectId="Dummy-123",
            Date=date.today(),
            TotalFrames=2500,
            Manufacture="DummyCorp"
        )

    # Metode til at hente punktdata fra C3D-filen
    def get_point_data(self, file_name: str):
        raise NotImplementedError("Method get_point_data() is not yet implemented")