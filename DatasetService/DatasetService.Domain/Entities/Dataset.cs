using DatasetService.Domain.ValueObjects;
using System.Reflection;

namespace DatasetService.Domain.Entities
{
    public class Dataset // Root Aggregate
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int NumberOfSubjects => _subjects.Count;
        public List<string> AllLabels => _allLabels;
        public IReadOnlyCollection<ContinuousVariable> ContinuousDataSummery => _continuousVariables;
        public IReadOnlyCollection<Subject> Subjects => _subjects;
        
        private readonly List<string> _allLabels = new();
        private readonly List<ContinuousVariable> _continuousVariables = new();
        private readonly List<Subject> _subjects = new();

        protected Dataset() { } // Required by EF Core

        private Dataset(string name, List<Subject> subjects, List<string> allLabels)
        {
            Id = Guid.NewGuid();
            Name = name;
            _subjects.AddRange(subjects);
            _allLabels.AddRange(allLabels);

            // Validering:
            AssureSufficientSubjects();
            // Descriptive statistics:
            CreateContinuousVariableStatistic();
        }

        public static Dataset Create(string name, List<Subject> subjects, List<string> allLabels)
        {
            return new Dataset(name, subjects, allLabels);
        }

        private void CreateContinuousVariableStatistic()
        {
            var subjectType = typeof(Subject);
            var numericProperties = subjectType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(int) ||
                            p.PropertyType == typeof(float) ||
                            p.PropertyType == typeof(double))
                .ToList();

            foreach (var prop in numericProperties)
            {
                var name = prop.Name;
                var values = _subjects
                    .Select(s => Convert.ToDouble(prop.GetValue(s)))
                    .ToList();

                double min = values.Min();
                double max = values.Max();
                double mean = values.Average();
                double median = GetMedian(values);
                double stdDev = GetStdDev(values);

                var variable = ContinuousVariable.Create(name, min, max, mean, median, stdDev);
                _continuousVariables.Add(variable);
            }
        }

        // ----------------------- Validation ------------------------   
        private void AssureSufficientSubjects()
        {
            if (_subjects.Count == 0)
                throw new ArgumentException("Dataset must contain at least one Subject.");
        }

        // ----------------------- Helper Methods ------------------------  
        private double GetMedian(List<double> values)
        {
            var sorted = values.OrderBy(v => v).ToList();
            int count = sorted.Count;
            return count % 2 == 0
                ? (sorted[count / 2 - 1] + sorted[count / 2]) / 2.0
                : sorted[count / 2];
        }

        private double GetStdDev(List<double> values)
        {
            var mean = values.Average();
            var variance = values.Sum(v => Math.Pow(v - mean, 2)) / values.Count;
            return Math.Sqrt(variance);
        }
    }
}
