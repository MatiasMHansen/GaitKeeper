using DatasetService.Application.Query;
using DatasetService.Application.Query.QueryDTOs;
using DatasetService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatasetService.Infrastructure.Query
{
    public class DatasetQuery : IDatasetQuery
    {
        private readonly DatasetContext _db;

        public DatasetQuery(DatasetContext db)
        {
            _db = db;
        }

        public async Task<List<QueryPreviewDatasetDTO>> GetAllAsync()
        {
            var previewDatasetDtos = await _db.Datasets
                .AsNoTracking()
                .Select(ds => new QueryPreviewDatasetDTO
                {
                    Id = ds.Id,
                    Name = ds.Name,
                    NumberOfSubjects = ds.NumberOfSubjects,
                    AllLabels = ds.AllLabels.ToList(),
                    ContinuousDataSummery = ds.ContinuousDataSummery
                        .Select(c => new ContinuousVariableDTO
                        {
                            Name = c.Name,
                            Min = c.Min,
                            Max = c.Max,
                            Mean = c.Mean,
                            Median = c.Median,
                            StdDev = c.StdDev
                        }).ToList()
                })
                .ToListAsync();

            return previewDatasetDtos;
        }

        async Task<QueryDatasetDTO> IDatasetQuery.GetAsync(Guid id)
        {
            var datasetDto = await _db.Datasets
            .AsNoTracking()
            .Where(ds => ds.Id == id)
            .Select(ds => new QueryDatasetDTO
            {
                Id = ds.Id,
                Name = ds.Name,
                NumberOfSubjects = ds.Subjects.Count,
                AllLabels = ds.AllLabels.ToList(),

                Subjects = ds.Subjects.Select(subject => new QuerySubjectDTO
                {
                    SubjectId = subject.SubjectId,
                    PointDataId = subject.PointDataId,
                    Description = subject.Description,
                    Sex = subject.Sex,
                    Age = subject.Age,
                    Height = subject.Height,
                    Weight = subject.Weight,
                    LLegLength = subject.LLegLength,
                    RLegLength = subject.RLegLength,
                }).ToList(),

                ContinuousDataSummery = ds.ContinuousDataSummery.Select(c => new ContinuousVariableDTO
                {
                    Name = c.Name,
                    Min = c.Min,
                    Max = c.Max,
                    Mean = c.Mean,
                    Median = c.Median,
                    StdDev = c.StdDev
                }).ToList()
            })
            .FirstOrDefaultAsync();

            return datasetDto;
        }
    }
}
