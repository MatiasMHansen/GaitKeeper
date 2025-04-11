using GaitSessionService.Application.Command.CommandDTOs;

namespace GaitSessionService.Application.Command
{
    public interface IGaitSessionCommand
    {
        Task CreateAsync(CreateGaitSessionDTO createGaitSessionDTO);
        Task DeleteAsync(Guid id);
    }
}
