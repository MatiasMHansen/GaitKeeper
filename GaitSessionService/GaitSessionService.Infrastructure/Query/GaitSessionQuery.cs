using GaitSessionService.Application.Query;
using GaitSessionService.Application.Query.QueryDTOs;
using Microsoft.EntityFrameworkCore;

namespace GaitSessionService.Infrastructure.Query
{
    public class GaitSessionQuery : IGaitSessionQuery
    {
        private readonly GaitSessionContext _db;

        public GaitSessionQuery(GaitSessionContext context)
        {
            _db = context;
        }

        async Task<GaitSessionQueryDTO> IGaitSessionQuery.GetAsync(Guid pointDataId)
        {
            //var gaitSession = await _db.GaitSessions.AsNoTracking().FirstOrDefaultAsync(gs => gs.PointDataId == pointDataId);

            //if (gaitSession == null)
            //{
            //    Console.WriteLine($"ATTENTION - Coundn't find any GaitSession with PointDataId: {pointDataId}.");
            //    return null;
            //}

            //return gaitSessionDTO;

            throw new NotImplementedException("GaitSessionQuery.GetAsync not implemented yet.");
        }

        async Task<List<GaitSessionQueryDTO>> IGaitSessionQuery.GetAllAsync()
        {
            //var gaitSessions = await _db.GaitSessions.AsNoTracking().ToListAsync();

            //if (gaitSessions == null || !gaitSessions.Any())
            //{
            //    Console.WriteLine("ATTENTION - GaitSessions not found.");
            //    return new List<GaitSessionQueryDTO>();
            //}

            //return gaitSessions;

            throw new NotImplementedException("GaitSessionQuery.GetAllAsync not implemented yet.");
        }
    }
}
