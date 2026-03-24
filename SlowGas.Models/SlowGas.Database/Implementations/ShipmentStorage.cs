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
    public class ShipmentStorage : IShipmentStorageContract
    {
        private readonly SlowGasDbContext _dbContext;
        private readonly IMapper _mapper;

        public ShipmentStorage(SlowGasDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public List<Shipment> GetList(DateTime? fromDate = null, DateTime? toDate = null, string? requestId = null)
        {
            try
            {
                var query = _dbContext.Shipments
                    .Include(s => s.Request)
                    .AsQueryable();

                if (fromDate.HasValue)
                {
                    query = query.Where(s => s.ShipmentDate >= fromDate.Value);
                }

                if (toDate.HasValue)
                {
                    query = query.Where(s => s.ShipmentDate <= toDate.Value);
                }

                if (!string.IsNullOrEmpty(requestId))
                {
                    query = query.Where(s => s.RequestId == requestId);
                }

                return query
                    .OrderByDescending(s => s.ShipmentDate)
                    .Select(s => _mapper.Map<Shipment>(s))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public Shipment? GetElementById(string id)
        {
            try
            {
                var entity = _dbContext.Shipments
                    .Include(s => s.Request)
                    .FirstOrDefault(s => s.Id == id);
                return entity == null ? null : _mapper.Map<Shipment>(entity);
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public void AddElement(Shipment shipment)
        {
            try
            {
                if (_dbContext.Shipments.Any(s => s.Id == shipment.Id))
                {
                    throw new ElementExistsException("Id", shipment.Id);
                }

                var request = _dbContext.Requests.Find(shipment.RequestId);
                if (request == null)
                {
                    throw new ElementNotFoundException($"Request {shipment.RequestId}");
                }

                var entity = _mapper.Map<ShipmentEntity>(shipment);
                entity.VersionDate = DateTime.UtcNow;
                _dbContext.Shipments.Add(entity);
                _dbContext.SaveChanges();
            }
            catch (ElementExistsException)
            {
                throw;
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

        public void CancelElement(string id)
        {
            try
            {
                var entity = _dbContext.Shipments.Find(id);
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