using DatasetService.Application.Query.QueryDTOs;
using DatasetService.Application.Utility.UtilDTOs;
using DatasetService.Domain.Entities;

namespace DatasetService.Application.Utility
{
    public interface IExportDataset
    {
        Task<string> PrintCharacteristicToCSV(Guid id);
        Task<string> PrintMarkerToCSV(QueryDatasetDTO dataset, List<PartialPointDataDTO> pointData, string markerLabel, char axis);
    }
}
