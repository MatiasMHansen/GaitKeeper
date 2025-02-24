from abc import ABC, abstractmethod

class IC3DParserService(ABC):
    @abstractmethod
    def get_metadata(self, file_name: str):
        pass

    @abstractmethod
    def get_point_data(self, file_name: str):
        pass
