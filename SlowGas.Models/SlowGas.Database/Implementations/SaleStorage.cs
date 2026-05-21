using Microsoft.EntityFrameworkCore;
using SlowGas.Database.Models;
using SlowGas.Models.DataModels;
using SlowGas.Models.Enums;
using SlowGas.Models.Exceptions;
using SlowGas.Models.StoragesContracts;
using Newtonsoft.Json;

namespace SlowGas.Database.Implementations
{
    public class SaleStorage : ISaleStorageContract
    {
        private readonly SlowGasDbContext _dbContext;

        public SaleStorage(SlowGasDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<SaleDataModel> GetList(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var entities = _dbContext.Sales
                    .Where(s => s.SaleDate >= fromDate && s.SaleDate <= toDate)
                    .ToList();

                return entities.Select(e => MapToModel(e)).ToList();
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public List<SaleDataModel> GetList(DateTime fromDate, DateTime toDate, string workerId, string customerId, bool? isReturned)
        {
            try
            {
                var query = _dbContext.Sales
                    .Where(s => s.SaleDate >= fromDate && s.SaleDate <= toDate);

                if (!string.IsNullOrEmpty(workerId))
                    query = query.Where(s => s.WorkerId == workerId);

                if (!string.IsNullOrEmpty(customerId))
                    query = query.Where(s => s.CustomerId == customerId);

                if (isReturned.HasValue)
                    query = query.Where(s => s.IsReturned == isReturned.Value);

                return query.Select(e => MapToModel(e)).ToList();
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public SaleDataModel GetElementById(string id)
        {
            try
            {
                var entity = _dbContext.Sales.FirstOrDefault(s => s.SaleId == id);
                return entity == null ? null : MapToModel(entity);
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public void AddElement(SaleDataModel model)
        {
            try
            {
                model.Validate();

                var entity = new SaleEntity
                {
                    SaleId = model.Id,
                    WorkerId = model.WorkerId,
                    CustomerId = model.CustomerId,
                    SaleDate = model.SaleDate,
                    DiscountType = (int)model.DiscountType,
                    IsReturned = model.IsReturned,
                    ProductsJson = JsonConvert.SerializeObject(model.Products)
                };

                _dbContext.Sales.Add(entity);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public void UpdateElement(SaleDataModel model)
        {
            try
            {
                model.Validate();

                var entity = _dbContext.Sales.FirstOrDefault(s => s.SaleId == model.Id);
                if (entity != null)
                {
                    entity.IsReturned = model.IsReturned;
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
                var entity = _dbContext.Sales.FirstOrDefault(s => s.SaleId == id);
                if (entity != null)
                {
                    _dbContext.Sales.Remove(entity);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        private SaleDataModel MapToModel(SaleEntity entity)
        {
            var products = string.IsNullOrEmpty(entity.ProductsJson)
                ? new List<SaleProductDataModel>()
                : JsonConvert.DeserializeObject<List<SaleProductDataModel>>(entity.ProductsJson);

            return new SaleDataModel(
                entity.SaleId,
                entity.WorkerId,
                entity.CustomerId,
                (DiscountType)entity.DiscountType,
                entity.IsReturned,
                products ?? new List<SaleProductDataModel>()
            );
        }
    }
}