namespace DatasetService.Domain.ValueObjects
{
    public class ContinuousVariable
    {
        public string Name { get; }
        public double Min { get; }
        public double Max { get; }
        public double Mean { get; }
        public double Median { get; }
        public double StdDev { get; }

        protected ContinuousVariable() { } // Required by EF Core

        private ContinuousVariable(string name, double min, double max, double mean, double median, double stdDev) 
        {
            Name = name;
            Min = min;
            Max = max;
            Mean = mean;
            Median = median;
            StdDev = stdDev;
        }

        public static ContinuousVariable Create(string name, double min, double max, double mean, double median, double stdDev) 
            => new(name, min, max, mean, median, stdDev);
    }
}
