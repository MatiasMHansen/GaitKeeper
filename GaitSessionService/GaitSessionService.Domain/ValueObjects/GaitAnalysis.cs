namespace GaitSessionService.Domain.ValueObjects
{
    public class GaitAnalysis
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Context { get; private set; }
        public string UnitType { get; private set; }
        public float Value { get; private set; }

        protected GaitAnalysis() { } // Required by EF Core

        private GaitAnalysis(string name, string description, string context, string unitType, float value)
        {
            Name = name;
            Description = description;
            Context = context;
            UnitType = unitType;
            Value = value;
        }

        public static GaitAnalysis Create(string name, string description, string context, string unitType, float value)
        {
            return new GaitAnalysis(name, description, context, unitType, value);
        }
    }
}
