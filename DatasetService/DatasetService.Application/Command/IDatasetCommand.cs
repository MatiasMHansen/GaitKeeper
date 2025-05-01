using DatasetService.Application.Command.CommandDTOs;

namespace DatasetService.Application.Command
{
    public interface IDatasetCommand
    {
        Task CreateAsync(CreateDatasetDTO datasetDTO);
    }
}
