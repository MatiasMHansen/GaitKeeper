using GaitPointData.Application.Command.CommandDTOs;

namespace GaitPointData.Application.Command
{
    public interface IPointDataCommand
    {
        Task CreateAsync(CreatePointDataDTO createPointDataDTO);
        Task DeleteAsync(Guid id);
    }
}
