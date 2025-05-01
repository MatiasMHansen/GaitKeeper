using DatasetService.Application.Command.CommandDTOs;
using DatasetService.Domain.Entities;

namespace DatasetService.Application.Command
{
    public class DatasetCommand : IDatasetCommand
    {
        private readonly IDatasetRepository _repo;

        public DatasetCommand(IDatasetRepository repo)
        {
            _repo = repo;
        }

        async Task IDatasetCommand.CreateAsync(CreateDatasetDTO datasetDTO)
        {
            var subjects = new List<Subject>();
            var allLabels = new List<string>();

            // Saml labels - Antager at alle Sessions har de samme labels
            var firstSession = datasetDTO.gaitSessions.FirstOrDefault();
            allLabels.AddRange(firstSession.AngleLabels);
            allLabels.AddRange(firstSession.ForceLabels);
            allLabels.AddRange(firstSession.ModeledLabels);
            allLabels.AddRange(firstSession.MomentLabels);
            allLabels.AddRange(firstSession.PowerLabels);
            allLabels.AddRange(firstSession.PointLabels);

            foreach (var session in datasetDTO.gaitSessions)
            {
                // Create the Subject entity
                var subject = Subject.Create(
                    subjectId: session.SubjectId,
                    pointDataId: session.PointDataId,
                    description: session.Description,
                    sex: session.Sex,
                    age: session.Age,
                    height: session.Biometrics.Height,
                    weight: session.Biometrics.Weight,
                    lLegLength: session.Biometrics.LLegLength,
                    rLegLength: session.Biometrics.RLegLength
                );

                subjects.Add(subject);
            }

            // Create the Dataset entity
            var dataset = Dataset.Create(datasetDTO.Name, subjects, allLabels);

            // Save the dataset to the repository
            await _repo.SaveAsync(dataset);
        }
    }
}
