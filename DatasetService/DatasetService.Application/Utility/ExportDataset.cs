using DatasetService.Application.Query;
using DatasetService.Application.Query.QueryDTOs;
using DatasetService.Application.Utility.UtilDTOs;
using DatasetService.Domain.Entities;
using System.Globalization;
using System.Text;

namespace DatasetService.Application.Utility
{
    public class ExportDataset : IExportDataset
    {
        private readonly IDatasetQuery _query;

        public ExportDataset(IDatasetQuery query)
        {
            _query = query;
        }

        async Task<string> IExportDataset.PrintCharacteristicToCSV(Guid id)
        {
            var dataset = await _query.GetAsync(id);
            var sb = new StringBuilder();

            // Header
            var header = new List<string>
            {
                "SubjectId",
                "Description",
                "Sex",
                "Age",
                "Height",
                "Weight",
                "LLegLength",
                "RLegLength",
            };
            sb.AppendLine(string.Join(",", header));

            // Rows
            foreach (var subject in dataset.Subjects)
            {
                var row = new List<string>
                {
                    subject.SubjectId,
                    subject.Description,
                    subject.Sex,
                    subject.Age.ToString(CultureInfo.InvariantCulture),
                    subject.Height.ToString(CultureInfo.InvariantCulture),
                    subject.Weight.ToString(CultureInfo.InvariantCulture),
                    subject.LLegLength.ToString(CultureInfo.InvariantCulture),
                    subject.RLegLength.ToString(CultureInfo.InvariantCulture),
                };
                sb.AppendLine(string.Join(",", row));
            }

            return sb.ToString();
        }

        async Task IExportDataset.PrintMarkerToCSV(QueryDatasetDTO dataset, List<PartialPointDataDTO> pointData, string markerLabel, char axis)
        {
            var sb = new StringBuilder();

            // Find det PartialPointDataDTO hvor den ønskede marker har flest Units
            var pointWithMostFrames = pointData
                .Where(dto => dto.Markers.Any(m => m.Label == markerLabel))
                .OrderByDescending(dto => dto.Markers.First(m => m.Label == markerLabel).Units.Count)
                .FirstOrDefault();

            int maxFrames = pointWithMostFrames?
                .Markers.First(m => m.Label == markerLabel)
                .Units.Count ?? 0;

            // Byg header
            var header = new List<string> { "SubjectId", "GaitCycle-Number", "Duration" };
            for (int i = 1; i <= maxFrames; i++)
            {
                header.Add($"Frame {i}");
            }
            sb.AppendLine(string.Join(",", header));

            // 3. Én række per pointData entry
            foreach (var subjectData in pointData)
            {
                var marker = subjectData.Markers.FirstOrDefault(m => m.Label == markerLabel);

                var axisValues = marker.Units.Select(unit =>
                {
                    return axis switch
                    {
                        'X' => unit.X,
                        'Y' => unit.Y,
                        'Z' => unit.Z,
                        _ => null
                    };
                }).ToList();

                var row = new List<string>
                {
                    subjectData.SubjectId,
                    subjectData.Number.ToString(),
                    subjectData.Duration.ToString(CultureInfo.InvariantCulture)
                };

                // Tilføj frame data (pad med tomme felter hvis færre end max)
                row.AddRange(axisValues
                    .Select(v => v?.ToString(CultureInfo.InvariantCulture) ?? "")
                    .Concat(Enumerable.Repeat("", maxFrames - axisValues.Count)));

                sb.AppendLine(string.Join(",", row));
            }


            // Gem CSV - !Skal ændres til at blive downloadet i browseren!
            var baseDir = AppContext.BaseDirectory;
            // Gå 4 niveauer op: bin → Debug → net9.0 → DatasetService.Application → DatasetService → GaitKeeper
            var solutionRoot = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", ".."));
            // Nu har du: D:\LokalRepos\GaitKeeper\
            var exportDir = Path.Combine(solutionRoot, "export");
            Directory.CreateDirectory(exportDir);

            var fileName = $"{markerLabel}-{axis}_{dataset.Name}.csv";
            var filePath = Path.Combine(exportDir, fileName);

            await File.WriteAllTextAsync(filePath, sb.ToString());
        }
    }
}
