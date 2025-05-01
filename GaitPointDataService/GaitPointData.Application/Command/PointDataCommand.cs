using GaitPointData.Application.Command.CommandDTOs;
using GaitPointData.Application.ValueObjectDTOs;
using GaitPointData.Domain.Aggregate;
using GaitPointData.Domain.ValueObjects;

namespace GaitPointData.Application.Command
{
    public class PointDataCommand : IPointDataCommand
    {
        private readonly IPointDataRepository _repo;

        public PointDataCommand(IPointDataRepository repository)
        {
            _repo = repository;
        }

        public async Task CreateAsync(CreatePointDataDTO dto)
        {
            // Do: Konverter CreatePointDataDTO til PointData via Domain laget
            var pointData = PointData.Create(
                id: dto.Id,
                fileName: dto.FileName,
                subjectId: dto.SubjectId,
                pointFreq: dto.PointFreq,
                startFrame: dto.StartFrame,
                endFrame: dto.EndFrame,
                totalFrames: dto.TotalFrames,
                pointMarkers: MapMarkers(dto.PointMarkers),
                angleMarkers: MapMarkers(dto.AngleMarkers),
                forceMarkers: MapMarkers(dto.ForceMarkers),
                modeledMarkers: MapMarkers(dto.ModeledMarkers),
                momentMarkers: MapMarkers(dto.MomentMarkers),
                powerMarkers: MapMarkers(dto.PowerMarkers),
                lGaitCycles: MapGaitCycles(dto.LGaitCycles),
                rGaitCycles: MapGaitCycles(dto.RGaitCycles)
            );


            // Save:
            await _repo.SaveAsync(pointData);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repo.DeleteAsync(id);
        }

        // ------------ Helper metoder -------------
        private List<Marker> MapMarkers(List<MarkerDTO> markerDtos)
        {
            if (markerDtos == null) return new();

            return markerDtos
                .Select(dto => Marker.Create(
                    label: dto.Label,
                    unitType: dto.UnitType,
                    units: dto.Units?
                        .Select(u => new Unit(u.X, u.Y, u.Z))
                        .ToList() ?? new List<Unit>()))
                .ToList();
        }

        private List<GaitCycle> MapGaitCycles(List<GaitCycleDTO> cycleDtos)
        {
            if (cycleDtos == null) return new();

            return cycleDtos
                .Select(dto => GaitCycle.Create(
                    name: dto.Name,
                    description: dto.Description,
                    number: dto.Number,
                    eventStart: dto.EventStart,
                    eventEnd: dto.EventEnd,
                    duration: dto.Duration,
                    startFrame: dto.StartFrame,
                    endFrame: dto.EndFrame))
                .ToList();
        }
    }
}
