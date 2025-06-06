﻿using GaitSessionService.Application.Query;
using GaitSessionService.Application.Query.QueryDTOs;
using GaitSessionService.Domain.Aggregate;
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

        async Task<QueryGaitSessionDTO> IGaitSessionQuery.GetAsync(Guid pointDataId)
        {
            var queryDto = await _db.GaitSessions
                .AsNoTracking()
                .FirstOrDefaultAsync(gs => gs.PointDataId == pointDataId);

            return queryDto == null ? null : MapToDTO(queryDto); // Ternary operation: "condition ? valueIfTrue : valueIfFalse;"
        }

        async Task<List<QueryGaitSessionDTO>> IGaitSessionQuery.GetAsync(List<Guid> pointDataIds)
        {
            var queryDto = await _db.GaitSessions
                .AsNoTracking()
                .Where(gs => pointDataIds.Contains(gs.PointDataId))
                .ToListAsync();

            return queryDto.Any() ? queryDto.Select(MapToDTO).ToList() : new List<QueryGaitSessionDTO>();
        }

        async Task<List<QueryGaitSessionDTO>> IGaitSessionQuery.GetAllAsync()
        {
            var queryDto = await _db.GaitSessions
                .AsNoTracking()
                .ToListAsync();

            return queryDto.Select(MapToDTO).ToList();
        }

        private QueryGaitSessionDTO MapToDTO(GaitSession gs)
        {
            return new QueryGaitSessionDTO
            {
                Id = gs.Id,
                PointDataId = gs.PointDataId,
                Description = gs.Description,
                Sex = gs.Sex,
                Age = gs.Age,
                FileName = gs.FileName,
                SubjectId = gs.SubjectId,
                PointFreq = gs.PointFreq,
                AnalogFreq = gs.AnalogFreq,
                StartFrame = gs.StartFrame,
                EndFrame = gs.EndFrame,
                TotalFrames = gs.TotalFrames,
                AngleLabels = gs.AngleLabels.ToList(),
                ForceLabels = gs.ForceLabels.ToList(),
                ModeledLabels = gs.ModeledLabels.ToList(),
                MomentLabels = gs.MomentLabels.ToList(),
                PowerLabels = gs.PowerLabels.ToList(),
                PointLabels = gs.PointLabels.ToList(),
                Biometrics = new BiometricsDTO
                {
                    Height = gs.Biometrics.Height,
                    Weight = gs.Biometrics.Weight,
                    LLegLength = gs.Biometrics.LLegLength,
                    RLegLength = gs.Biometrics.RLegLength
                },
                SystemInfo = new SystemInfoDTO
                {
                    Software = gs.SystemInfo.Software,
                    Version = gs.SystemInfo.Version,
                    MarkerSetup = gs.SystemInfo.MarkerSetup
                },
                LGaitCycles = gs.LGaitCycles.Select(c => new GaitCycleDTO
                {
                    Name = c.Name,
                    Description = c.Description,
                    Number = c.Number,
                    StartFrame = c.StartFrame,
                    EndFrame = c.EndFrame,
                    EventStart = c.EventStart,
                    EventEnd = c.EventEnd,
                    Duration = c.Duration
                }).ToList(),
                RGaitCycles = gs.RGaitCycles.Select(c => new GaitCycleDTO
                {
                    Name = c.Name,
                    Description = c.Description,
                    Number = c.Number,
                    StartFrame = c.StartFrame,
                    EndFrame = c.EndFrame,
                    EventStart = c.EventStart,
                    EventEnd = c.EventEnd,
                    Duration = c.Duration
                }).ToList(),
                GaitAnalyses = gs.GaitAnalyses?.Select(a => new GaitAnalysisDTO
                {
                    Name = a.Name,
                    Description = a.Description,
                    Context = a.Context,
                    UnitType = a.UnitType,
                    Value = a.Value
                }).ToList()
            };
        }
    }
}
