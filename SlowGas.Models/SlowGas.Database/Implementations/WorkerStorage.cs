using Microsoft.EntityFrameworkCore;
using SlowGas.Database.Models;
using SlowGas.Models.DataModels;
using SlowGas.Models.Exceptions;
using SlowGas.Models.StoragesContracts;

namespace SlowGas.Database.Implementations
{
    public class WorkerStorage : IWorkerStorageContract
    {
        private readonly SlowGasDbContext _dbContext;

        public WorkerStorage(SlowGasDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<WorkerDataModel> GetList()
        {
            try
            {
                var entities = _dbContext.Workers
                    .Where(w => !w.IsDeleted)
                    .ToList();

                return entities.Select(e => new WorkerDataModel(
                    e.WorkerId,
                    e.FullName,
                    e.PostId,
                    e.EmploymentDate,
                    e.DateOfDelete ?? DateTime.MaxValue,
                    e.IsDeleted
                )).ToList();
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public WorkerDataModel GetElementById(string id)
        {
            try
            {
                var entity = _dbContext.Workers.FirstOrDefault(w => w.WorkerId == id && !w.IsDeleted);
                if (entity == null)
                    return null;

                return new WorkerDataModel(
                    entity.WorkerId,
                    entity.FullName,
                    entity.PostId,
                    entity.EmploymentDate,
                    entity.DateOfDelete ?? DateTime.MaxValue,
                    entity.IsDeleted
                );
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public void AddElement(WorkerDataModel model)
        {
            try
            {
                var entity = new WorkerEntity
                {
                    WorkerId = model.Id,
                    FullName = model.FullName,
                    PostId = model.PostId,
                    EmploymentDate = model.EmploymentDate,
                    IsDeleted = false
                };

                _dbContext.Workers.Add(entity);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public void UpdateElement(WorkerDataModel model)
        {
            try
            {
                var entity = _dbContext.Workers.FirstOrDefault(w => w.WorkerId == model.Id);
                if (entity != null)
                {
                    entity.FullName = model.FullName;
                    entity.PostId = model.PostId;
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public void DeleteElement(string id)
        {
            try
            {
                var entity = _dbContext.Workers.FirstOrDefault(w => w.WorkerId == id);
                if (entity != null)
                {
                    entity.IsDeleted = true;
                    entity.DateOfDelete = DateTime.Now;
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public int GetWorkerTrend(DateTime fromPeriod, DateTime toPeriod)
        {
            try
            {
                var countWorkersOnBeginning = _dbContext.Workers.Count(x =>
                    x.EmploymentDate < fromPeriod && (!x.IsDeleted || x.DateOfDelete > fromPeriod));

                var countWorkersOnEnding = _dbContext.Workers.Count(x =>
                    x.EmploymentDate < toPeriod && (!x.IsDeleted || x.DateOfDelete > toPeriod));

                return countWorkersOnEnding - countWorkersOnBeginning;
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }
    }
}