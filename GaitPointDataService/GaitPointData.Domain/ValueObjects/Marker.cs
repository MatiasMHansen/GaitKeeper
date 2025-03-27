namespace GaitPointData.Domain.ValueObjects
{
    public class Marker
    {
        public string Label { get; }
        public string UnitType { get; }
        private readonly List<Unit> _units = new();
        public IReadOnlyCollection<Unit> Units => _units;

        private Marker() { }

        private Marker(string label, string unitType, List<Unit> units)
        {
            Label = label;
            UnitType = unitType;
            _units.AddRange(units);
        }

        public static Marker Create(string label, string unitType, List<Unit> units)
        {
            return new Marker(label, unitType, units);
        }
    }
}
