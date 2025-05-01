using GaitPointData.Application.Query.QueryDTOs;
using GaitPointData.Application.ValueObjectDTOs;
using GaitPointData.Domain.ValueObjects;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace GaitPointData.Application.Query
{
    public class PartialPointDataQuery : IPartialPointDataQuery
    {
        private readonly IPointDataRepository _repo;

        public PartialPointDataQuery(IPointDataRepository repository)
        {
            _repo = repository;
        }

        // Benytter throttling + async + parallel CPU mapping for performance
        async Task<List<QueryPartialPointDataDTO>> IPartialPointDataQuery.GetAsync(List<Guid> ids, List<string> labels)
        {   
            var results = new List<QueryPartialPointDataDTO>();
            var tasks = new List<Task>();

            var semaphore = new SemaphoreSlim(20); // Throttling max 20 samtidige tasks

            foreach (var id in ids)
            {
                await semaphore.WaitAsync(); // Throttle parallel load af PointData

                var task = Task.Run(async () =>
                {
                    try
                    {
                        var queryPointDataDTO = await _repo.LoadAsync(id);
                        if (queryPointDataDTO == null) return;

                        var (gaitCycle, startFrame, endFrame) = GetMiddleFrameRange(queryPointDataDTO.RGaitCycles, queryPointDataDTO.StartFrame);
                        var allMarkers = GetAllMarkers(queryPointDataDTO);

                        var filteredMarkers = new ConcurrentBag<MarkerDTO>(); // En trådsikker collection, flere tråde kan tilføjer elementer samtidig

                        // CPU-optimeret mapping af markører med Parallel.ForEach
                        Parallel.ForEach(allMarkers, marker =>
                        {
                            if (!labels.Contains(marker.Label)) return; // Filtrer markører baseret på labels

                            var slicedUnits = marker.Units // Henter kun målingerne for det midterste gangcyklus
                                .Skip(startFrame - 1)
                                .Take(endFrame - startFrame + 1)
                                .Select(u => new UnitDTO
                                {
                                    X = u.X,
                                    Y = u.Y,
                                    Z = u.Z
                                }).ToList();

                            filteredMarkers.Add(new MarkerDTO
                            {
                                Label = marker.Label,
                                UnitType = marker.UnitType,
                                Units = slicedUnits
                            });
                        });

                        var dto = new QueryPartialPointDataDTO
                        {
                            Id = queryPointDataDTO.Id,
                            FileName = queryPointDataDTO.FileName,
                            SubjectId = queryPointDataDTO.SubjectId,
                            PointFreq = queryPointDataDTO.PointFreq,
                            Duration = gaitCycle.Duration,
                            Number = gaitCycle.Number,
                            Markers = filteredMarkers.ToList()
                        };

                        lock (results)
                        {
                            results.Add(dto); // Trådsikker samling
                        }
                    }
                    finally
                    {
                        semaphore.Release(); // Frigiv plads i throttling
                    }
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks); // Afvent alle parallelle opgaver
            return results;
        }

        private static List<MarkerDTO> GetAllMarkers(QueryPointDataDTO dto)
        {
            return dto.PointMarkers
                .Concat(dto.AngleMarkers)
                .Concat(dto.ForceMarkers)
                .Concat(dto.ModeledMarkers)
                .Concat(dto.MomentMarkers)
                .Concat(dto.PowerMarkers)
                .ToList();
        }

        private static (GaitCycleDTO gaitCycle, int startFrame, int endFrame) GetMiddleFrameRange(List<GaitCycleDTO> gaitCycles, int sessionStartFrame)
        {
            var midIndex = gaitCycles.Count / 2;
            var gaitCycle = gaitCycles.ElementAt(midIndex);

            var startFrame = gaitCycle.StartFrame - sessionStartFrame;
            var endFrame = gaitCycle.EndFrame - sessionStartFrame;

            return (gaitCycle, startFrame, endFrame);
        }
    }
}
