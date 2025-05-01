namespace DatasetService.Domain.Entities
{
    public class Subject
    {
        public string SubjectId { get; private set; }
        public Guid PointDataId { get; private set; } // FK til PointData
        public string Description { get; private set; }
        public string Sex { get; private set; }
        public int Age { get; private set; }
        public float Height { get; private set; }
        public float Weight { get; private set; }
        public float LLegLength { get; private set; }
        public float RLegLength { get; private set; }

        protected Subject() { } // Required by EF Core

        private Subject(
            string subjectId, Guid pointDataId, string description, string sex, int age, float height, float weight,
            float lLegLength, float rLegLength)
        {
            SubjectId = subjectId;
            PointDataId = pointDataId;
            Description = description;
            Sex = sex;
            Age = age;
            Height = height;
            Weight = weight;
            LLegLength = lLegLength;
            RLegLength = rLegLength;
        }

        public static Subject Create(
            string subjectId, Guid pointDataId, string description, string sex, int age, float height, float weight,
            float lLegLength, float rLegLength)
        {
            var instance = new Subject(subjectId, pointDataId, description, sex, age, height, weight, lLegLength, rLegLength);
    
            return instance;
        }
    }
}
