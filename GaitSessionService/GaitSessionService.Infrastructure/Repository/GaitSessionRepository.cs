using GaitSessionService.Application;
using GaitSessionService.Domain.Aggregate;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GaitSessionService.Infrastructure.Repository
{
    public class GaitSessionRepository : IGaitSessionRepository
    {
        private readonly GaitSessionContext _db;
        private readonly ILogger<GaitSessionRepository> _log;

        public GaitSessionRepository(GaitSessionContext context, ILogger<GaitSessionRepository> logger)
        {
            _db = context;
            _log = logger;
        }

        async Task<bool> IGaitSessionRepository.ExistsAsync(Guid pointDataId)
        {   // Not in use atm
            return await _db.GaitSessions.AnyAsync(gs => gs.PointDataId == pointDataId);
        }

        async Task IGaitSessionRepository.SaveAsync(GaitSession gaitSession)
        { 
            try
            {
                await _db.GaitSessions.AddAsync(gaitSession);
                await _db.SaveChangesAsync();

                _log.LogInformation($"Success! GaitSession with PointDataId: {gaitSession.PointDataId} saved.");
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx &&
                                                (sqlEx.Number == 2601 || // Cannot insert duplicate key row in object with unique index
                                                sqlEx.Number == 2627))  // Violation of UNIQUE KEY constraint
            {
                _log.LogWarning($"WARNING - GaitSession with PointDataId: {gaitSession.PointDataId} already exists. Ignore action.");
            }
            catch (Exception ex)
            {
                _log.LogError($"EXCEPTION - GaitSession with PointDataId: {gaitSession.PointDataId} could not be created. {ex.Message}");
            }
        }

        async Task IGaitSessionRepository.DeleteAsync(Guid pointDataId)
        {
            var entity = await _db.GaitSessions.FirstOrDefaultAsync(gs => gs.PointDataId == pointDataId);

            if (entity == null)
            {
                _log.LogWarning($"WARNING - GaitSession with PointDataId: {pointDataId} was not found. No action taken.");
                return;
            }

            try
            {
                _db.GaitSessions.Remove(entity);
                await _db.SaveChangesAsync();

                _log.LogInformation($"Success! GaitSession with PointDataId: {pointDataId} deleted.");
            }
            catch (Exception ex)
            {
                _log.LogError($"ERROR - GaitSession with PointDataId: {pointDataId} could not be deleted. {ex.Message}");
            }
        }
    }
}
