using Microsoft.EntityFrameworkCore;
using SlowGas.Database.Models;
using SlowGas.Models.DataModels;
using SlowGas.Models.Exceptions;
using SlowGas.Models.StoragesContracts;

namespace SlowGas.Database.Implementations
{
    public class SalaryStorage : ISalaryStorageContract
    {
        private readonly SlowGasDbContext _dbContext;

        public SalaryStorage(SlowGasDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<SalaryDataModel> GetList(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var entities = _dbContext.Salaries
                    .Where(s => s.Period >= fromDate && s.Period <= toDate)
                    .ToList();

                return entities.Select(e => new SalaryDataModel(
                    e.SalaryId,
                    e.WorkerId,
                    e.Period,
                    (double)e.Amount,
                    e.CalculationDate
                )).ToList();
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public List<SalaryDataModel> GetList(DateTime fromDate, DateTime toDate, string workerId)
        {
            try
            {
                var entities = _dbContext.Salaries
                    .Where(s => s.Period >= fromDate && s.Period <= toDate && s.WorkerId == workerId)
                    .ToList();

                return entities.Select(e => new SalaryDataModel(
                    e.SalaryId,
                    e.WorkerId,
                    e.Period,
                    (double)e.Amount,
                    e.CalculationDate
                )).ToList();
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public SalaryDataModel GetElementById(string id)
        {
            try
            {
                var entity = _dbContext.Salaries.FirstOrDefault(s => s.SalaryId == id);

                if (entity == null)
                    return null;

                return new SalaryDataModel(
                    entity.SalaryId,
                    entity.WorkerId,
                    entity.Period,
                    (double)entity.Amount,
                    entity.CalculationDate
                );
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public void AddElement(SalaryDataModel model)
        {
            try
            {
                model.Validate();

                var entity = new SalaryEntity
                {
                    SalaryId = model.Id,
                    WorkerId = model.WorkerId,
                    Period = model.Period,
                    Amount = (decimal)model.Salary,
                    CalculationDate = model.CalculationDate
                };

                _dbContext.Salaries.Add(entity);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public void UpdateElement(SalaryDataModel model)
        {
            try
            {
                model.Validate();

                var entity = _dbContext.Salaries.FirstOrDefault(s => s.SalaryId == model.Id);
                if (entity != null)
                {
                    entity.Amount = (decimal)model.Salary;
                    entity.CalculationDate = model.CalculationDate;
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
                var entity = _dbContext.Salaries.FirstOrDefault(s => s.SalaryId == id);
                if (entity != null)
                {
                    _dbContext.Salaries.Remove(entity);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }
    }
}