using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SlowGas.Contracts.StoragesContracts;
using SlowGas.Database.Models;
using SlowGas.Models.DataModels;
using SlowGas.Models.Enums;
using SlowGas.Models.Exceptions;
using SlowGas.Models.StoragesContracts;

namespace SlowGas.Database.Implementations
{
    public class RequestStorage : IRequestStorageContract
    {
        private readonly SlowGasDbContext _dbContext;
        private readonly IMapper _mapper;

        public RequestStorage(SlowGasDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public List<Request> GetList(DateTime? fromDate = null, DateTime? toDate = null, string? customerId = null)
        {
            try
            {
                var query = _dbContext.Requests.AsQueryable();

                if (fromDate.HasValue)
                {
                    query = query.Where(r => r.RequestDate >= fromDate.Value);
                }

                if (toDate.HasValue)
                {
                    query = query.Where(r => r.RequestDate <= toDate.Value);
                }

                if (!string.IsNullOrEmpty(customerId))
                {
                    query = query.Where(r => r.CustomerId == customerId);
                }

                return query
                    .OrderByDescending(r => r.RequestDate)
                    .Select(r => _mapper.Map<Request>(r))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public Request? GetElementById(string id)
        {
            try
            {
                var entity = _dbContext.Requests.Find(id);
                return entity == null ? null : _mapper.Map<Request>(entity);
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public void AddElement(Request request)
        {
            try
            {
                if (_dbContext.Requests.Any(r => r.Id == request.Id))
                {
                    throw new ElementExistsException("Id", request.Id);
                }

                var entity = _mapper.Map<RequestEntity>(request);
                _dbContext.Requests.Add(entity);
                _dbContext.SaveChanges();
            }
            catch (ElementExistsException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public void CancelElement(string id)
        {
            try
            {
                var entity = _dbContext.Requests.Find(id);
                if (entity == null)
                {
                    throw new ElementNotFoundException(id);
                }

                entity.Status = OrderStatus.Cancelled;
                _dbContext.SaveChanges();
            }
            catch (ElementNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }
    }
}