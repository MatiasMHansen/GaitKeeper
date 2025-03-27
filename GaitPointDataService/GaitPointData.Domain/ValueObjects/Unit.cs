namespace GaitPointData.Domain.ValueObjects
{
    public class Unit
    {
        public float? X { get; }
        public float? Y { get; }
        public float? Z { get; }

        private Unit() { }

        public Unit(float? x, float? y, float? z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
