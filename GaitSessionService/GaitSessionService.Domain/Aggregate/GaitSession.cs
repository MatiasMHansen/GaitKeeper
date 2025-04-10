using GaitSessionService.Domain.ValueObjects;

namespace GaitSessionService.Domain.Aggregate
{
    public class GaitSession
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid PointDataId { get; private set; }
        // Brugergenererede data
        public string Description { get; private set; }
        public string Sex { get; private set; }
        public int Age { get; private set; }
        // Metadata
        public string FileName { get; private set; }
        public string SubjectId { get; private set; }
        public float PointFreq { get; private set; }
        public float AnalogFreq { get; private set; }
        public int StartFrame { get; private set; }
        public int EndFrame { get; private set; }
        public int TotalFrames { get; private set; }
        // Value Objects
        public Biometrics Biometrics { get; private set; }
        public SystemInfo SystemInfo { get; private set; }
        public List<string> AngleLabels => _angleLabels;
        public List<string> ForceLabels => _forceLabels;
        public List<string> ModeledLabels => _modeledLabels;
        public List<string> MomentLabels => _momentLabels;
        public List<string> PowerLabels => _powerLabels;
        public List<string> PointLabels => _pointLabels;
        public IReadOnlyCollection<GaitCycle> LGaitCycles => _lGaitCycles;
        public IReadOnlyCollection<GaitCycle> RGaitCycles => _rGaitCycles;
        public IReadOnlyCollection<GaitAnalysis> GaitAnalyses => _gaitAnalyses;

        // Private readonly fields
        private readonly List<string> _angleLabels = new();
        private readonly List<string> _forceLabels = new();
        private readonly List<string> _modeledLabels = new();
        private readonly List<string> _momentLabels = new();
        private readonly List<string> _powerLabels = new();
        private readonly List<string> _pointLabels = new();
        private readonly List<GaitCycle> _lGaitCycles = new();
        private readonly List<GaitCycle> _rGaitCycles = new();
        private readonly List<GaitAnalysis> _gaitAnalyses = new();

        protected GaitSession() { } // Required by EF Core

        private GaitSession(
            Guid pointDataId,
            string description, string sex, int age,
            string fileName, string subjectId, float pointFreq, float analogFreq, int startFrame, int endFrame, int totalFrames,
            Biometrics biometrics, SystemInfo systemInfo)
        {
            PointDataId = pointDataId;
            Description = description;
            Sex = sex;
            Age = age;
            FileName = fileName;
            SubjectId = subjectId;
            PointFreq = pointFreq;
            AnalogFreq = analogFreq;
            StartFrame = startFrame;
            EndFrame = endFrame;
            TotalFrames = totalFrames;
            Biometrics = biometrics;
            SystemInfo = systemInfo;

            // Validering:
            AssureSomething();
        }

        public static GaitSession Create(
            Guid pointDataId,
            string description, string sex, int age,
            string fileName, string subjectId, float pointFreq, float analogFreq, int startFrame, int endFrame, int totalFrames,
            List<string> angleLabels, List<string> forceLabels, List<string> modeledLabels, List<string> momentLabels, List<string> powerLabels, List<string> pointLabels,
            Biometrics biometrics, SystemInfo systemInfo,
            List<GaitCycle> lGaitCycles, List<GaitCycle> rGaitCycles, List<GaitAnalysis>? gaitAnalyses)
        {
            var instance = new GaitSession(
                pointDataId, 
                description, sex, age, fileName, subjectId, pointFreq, analogFreq, startFrame, endFrame, totalFrames,
                biometrics, systemInfo);

            instance._angleLabels.AddRange(angleLabels);
            instance._forceLabels.AddRange(forceLabels);
            instance._modeledLabels.AddRange(modeledLabels);
            instance._momentLabels.AddRange(momentLabels);
            instance._powerLabels.AddRange(powerLabels);
            instance._pointLabels.AddRange(pointLabels);
            instance._lGaitCycles.AddRange(lGaitCycles);
            instance._rGaitCycles.AddRange(rGaitCycles);
            if (gaitAnalyses != null)
                instance._gaitAnalyses.AddRange(gaitAnalyses);

            return instance;
        }

        private void AssureSomething()
        {
            // Validering
        }
    }
}
