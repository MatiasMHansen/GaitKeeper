using GaitSessionService.Domain.Aggregate;

namespace GaitSessionService.Application
{
    public interface IGaitSessionRepository
    {
        Task<bool> ExistsAsync(Guid pointDataId);

        Task SaveAsync(GaitSession gaitSession);

        Task DeleteAsync(Guid id);
    }
}
