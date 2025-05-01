using DatasetService.Application;
using DatasetService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DatasetService.Infrastructure.Repository
{
    public class DatasetRepository : IDatasetRepository
    {
        private readonly DatasetContext _db;
        private readonly ILogger<DatasetRepository> _log;

        public DatasetRepository(DatasetContext db, ILogger<DatasetRepository> logger)
        {
            _db = db;
            _log = logger;
        }

        async Task IDatasetRepository.SaveAsync(Dataset dataset)
        {
            try
            {
                await _db.Datasets.AddAsync(dataset);
                await _db.SaveChangesAsync();

                _log.LogInformation($"Success! {dataset.Name} have been saved.");
            }
            catch (Exception ex)
            {
                _log.LogError($"EXCEPTION - Failed to save {dataset.Name}. {ex.Message}");
                throw;
            }
        }
    }
}
