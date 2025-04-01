using GaitSessionService.Application.Command.CommandDTOs;
using GaitSessionService.Domain.Aggregate;
using GaitSessionService.Domain.ValueObjects;

namespace GaitSessionService.Application.Command
{
    public class GaitSessionCommand : IGaitSessionCommand
    {
        private readonly IGaitSessionRepository _repo;

        public GaitSessionCommand(IGaitSessionRepository repository)
        {
            _repo = repository;
        }

        async Task IGaitSessionCommand.CreateAsync(CreateGaitSessionDTO dto)
        { 
            // Do: Konverter CreateGaitSessionDTO til GaitSession via Domain laget
            var gaitSession = GaitSession.Create(
                pointDataId: dto.PointDataId.Value,

                description: dto.Description,
                sex: dto.Sex,
                age: dto.Age,

                fileName: dto.FileName,
                subjectId: dto.SubjectId,
                pointFreq: dto.PointFreq,
                analogFreq: dto.AnalogFreq,
                startFrame: dto.StartFrame,
                endFrame: dto.EndFrame,
                totalFrames: dto.TotalFrames,

                angleLabels: dto.AngleLabels,
                forceLabels: dto.ForceLabels,
                modeledLabels: dto.ModeledLabels,
                momentLabels: dto.MomentLabels,
                powerLabels: dto.PowerLabels,
                pointLabels: dto.PointLabels,

                biometrics: MapBiometrics(dto.Biometrics),
                systemInfo: MapSystemInfo(dto.SystemInfo),
                lGaitCycles: MapGaitCycles(dto.LGaitCycles),
                rGaitCycles: MapGaitCycles(dto.RGaitCycles),
                gaitAnalyses: MapGaitAnalyses(dto.GaitAnalyses)
            );

            // Save:
            await _repo.SaveAsync(gaitSession);
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        // ------------ Helper metoder -------------
        private Biometrics MapBiometrics(BiometricsDTO dto)
        {
            return Biometrics.Create(
                height: dto.Height,
                weight: dto.Weight,
                lLegLength: dto.LLegLength,
                rLegLength: dto.RLegLength);

        }

        private SystemInfo MapSystemInfo(SystemInfoDTO dto)
        {
            return SystemInfo.Create(
                software: dto.Software,
                version: dto.Version,
                markerSetup: dto.MarkerSetup);
        }

        private List<GaitCycle> MapGaitCycles(List<GaitCycleDTO> dtos)
        {
            return dtos
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

        private List<GaitAnalysis> MapGaitAnalyses(List<GaitAnalysisDTO> dtos)
        {
            if (dtos == null)
                return new List<GaitAnalysis>();

            return dtos
                .Select(dto => GaitAnalysis.Create(
                    name: dto.Name,
                    description: dto.Description,
                    context: dto.Context,
                    unitType: dto.UnitType,
                    value: dto.Value))
                .ToList();
        }
    }
}
