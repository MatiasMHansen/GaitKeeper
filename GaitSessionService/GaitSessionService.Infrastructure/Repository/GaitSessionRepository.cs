using GaitSessionService.Application;
using GaitSessionService.Domain;
using GaitSessionService.Domain.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace GaitSessionService.Infrastructure.Repository
{
    public class GaitSessionRepository : IGaitSessionRepository
    {
        private readonly GaitSessionContext _db;

        public GaitSessionRepository(GaitSessionContext context)
        {
            _db = context;
        }

        async Task IGaitSessionRepository.SaveAsync(GaitSession gaitSession)
        {
            await _db.GaitSessions.AddAsync(gaitSession);
            await _db.SaveChangesAsync();
        }

        async Task IGaitSessionRepository.DeleteAsync(Guid pointDataId)
        {
            var entity = await _db.GaitSessions
                .FirstOrDefaultAsync(gs => gs.PointDataId == pointDataId);

            if (entity is null)
                return;

            _db.GaitSessions.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
