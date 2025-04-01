namespace GaitSessionService.Domain.ValueObjects
{
    public class Biometrics
    {
        public float Height { get; private set; }
        public float Weight { get; private set; }
        public float LLegLength { get; private set; }
        public float RLegLength { get; private set; }

        protected Biometrics() { } // Required by EF Core

        private Biometrics(float height, float weight, float lLegLength, float rLegLength)
        {
            Height = height;
            Weight = weight;
            LLegLength = lLegLength;
            RLegLength = rLegLength;
        }

        public static Biometrics Create(float height, float weight, float lLegLength, float rLegLength)
        {
            return new Biometrics(height, weight, lLegLength, rLegLength);
        }
    }
}
