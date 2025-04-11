namespace GaitPointData.Domain.ValueObjects
{
    public class GaitCycle
    {
        public string Name { get; }
        public string Description { get; }
        public int Number { get; }
        public double EventStart { get; }
        public double EventEnd { get; }
        public double Duration { get; }
        public int StartFrame { get; }
        public int EndFrame { get; }

        private GaitCycle() { }

        private GaitCycle(string name, string description, int number, double eventStart, double eventEnd, double duration, int startFrame, int endFrame)
        {
            Name = name;
            Description = description;
            Number = number;
            EventStart = eventStart;
            EventEnd = eventEnd;
            Duration = duration;
            StartFrame = startFrame;
            EndFrame = endFrame;
        }

        public static GaitCycle Create(string name, string description, int number, double eventStart, double eventEnd, double duration, int startFrame, int endFrame)
        {
            return new GaitCycle(name, description, number, eventStart, eventEnd, duration, startFrame, endFrame);
        }
    }
}
