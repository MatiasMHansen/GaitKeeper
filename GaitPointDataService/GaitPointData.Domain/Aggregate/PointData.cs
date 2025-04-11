using GaitPointData.Domain.ValueObjects;

namespace GaitPointData.Domain.Aggregate
{
    public class PointData
    {
        public Guid Id { get; private set; } // Bruger correlationId fra Orchestrator
        public string FileName { get; private set; }
        public string SubjectId { get; private set; }
        public double PointFreq { get; private set; }
        public int StartFrame { get; private set; }
        public int EndFrame { get; private set; }
        public int TotalFrames { get; private set; }
        // Value Objects
        public IReadOnlyCollection<Marker> PointMarkers => _pointMarkers;
        public IReadOnlyCollection<Marker> AngleMarkers => _angleMarkers;
        public IReadOnlyCollection<Marker> ForceMarkers => _forceMarkers;
        public IReadOnlyCollection<Marker> ModeledMarkers => _modeledMarkers;
        public IReadOnlyCollection<Marker> MomentMarkers => _momentMarkers;
        public IReadOnlyCollection<Marker> PowerMarkers => _powerMarkers;
        public IReadOnlyCollection<GaitCycle> LGaitCycles => _lGaitCycles;
        public IReadOnlyCollection<GaitCycle> RGaitCycles => _rGaitCycles;

        // Private readonly fields
        private readonly List<Marker> _pointMarkers = new();
        private readonly List<Marker> _angleMarkers = new();
        private readonly List<Marker> _forceMarkers = new();
        private readonly List<Marker> _modeledMarkers = new();
        private readonly List<Marker> _momentMarkers = new();
        private readonly List<Marker> _powerMarkers = new();
        private readonly List<GaitCycle> _lGaitCycles = new();
        private readonly List<GaitCycle> _rGaitCycles = new();

        protected PointData() { }

        private PointData(
            Guid id, string fileName, string subjectId, double pointFreq,
            int startFrame, int endFrame, int totalFrames)
        {
            Id = id;
            FileName = fileName;
            SubjectId = subjectId;
            PointFreq = pointFreq;
            StartFrame = startFrame;
            EndFrame = endFrame;
            TotalFrames = totalFrames;

            // Validering:
            AssureInvariants();
        }

        public static PointData Create(
            Guid id, string fileName, string subjectId, double pointFreq,
            int startFrame, int endFrame, int totalFrames,
            List<Marker> pointMarkers, List<Marker> angleMarkers, List<Marker> forceMarkers,
            List<Marker> modeledMarkers, List<Marker> momentMarkers, List<Marker> powerMarkers,
            List<GaitCycle> lGaitCycles,  List<GaitCycle> rGaitCycles)
        {
            var instance = new PointData(id, fileName, subjectId, pointFreq, startFrame, endFrame, totalFrames);

            instance._pointMarkers.AddRange(pointMarkers);
            instance._angleMarkers.AddRange(angleMarkers);
            instance._forceMarkers.AddRange(forceMarkers);
            instance._modeledMarkers.AddRange(modeledMarkers);
            instance._momentMarkers.AddRange(momentMarkers);
            instance._powerMarkers.AddRange(powerMarkers);
            instance._lGaitCycles.AddRange(lGaitCycles);
            instance._rGaitCycles.AddRange(rGaitCycles);

            return instance;
        }

        private void AssureInvariants()
        {
            if (PointFreq <= 0)
                throw new ArgumentException("PointFreq must be greater than 0");

            if (StartFrame >= EndFrame)
                throw new ArgumentException("StartFrame must be before EndFrame");
        }
    }
}
