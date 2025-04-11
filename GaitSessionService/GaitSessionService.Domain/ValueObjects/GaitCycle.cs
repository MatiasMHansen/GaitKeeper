namespace GaitSessionService.Domain.ValueObjects
{
    public class GaitCycle
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Number { get; private set; }
        public int StartFrame { get; private set; }
        public int EndFrame { get; private set; }
        public float EventStart { get; private set; }
        public float EventEnd { get; private set; }
        public float Duration { get; private set; }

        protected GaitCycle() { } // Required by EF Core

        private GaitCycle(string name, string description, int number, int startFrame, int endFrame, float eventStart, float eventEnd, float duration)
        {
            Name = name;
            Description = description;
            Number = number;
            StartFrame = startFrame;
            EndFrame = endFrame;
            EventStart = eventStart;
            EventEnd = eventEnd;
            Duration = duration;
        }

        public static GaitCycle Create(string name, string description, int number, int startFrame, int endFrame, float eventStart, float eventEnd, float duration)
        {
            return new GaitCycle(name, description, number, startFrame, endFrame, eventStart, eventEnd, duration);
        }
    }
}
