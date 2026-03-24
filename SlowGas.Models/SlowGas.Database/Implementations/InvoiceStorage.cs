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
    public class InvoiceStorage : IInvoiceStorageContract
    {
        private readonly SlowGasDbContext _dbContext;
        private readonly IMapper _mapper;

        public InvoiceStorage(SlowGasDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public List<Invoice> GetList(DateTime? fromDate = null, DateTime? toDate = null, string? shipmentId = null)
        {
            try
            {
                var query = _dbContext.Invoices
                    .Include(i => i.Customer)
                    .Include(i => i.Shipment)
                    .AsQueryable();

                if (fromDate.HasValue)
                {
                    query = query.Where(i => i.InvoiceDate >= fromDate.Value);
                }

                if (toDate.HasValue)
                {
                    query = query.Where(i => i.InvoiceDate <= toDate.Value);
                }

                if (!string.IsNullOrEmpty(shipmentId))
                {
                    query = query.Where(i => i.ShipmentId == shipmentId);
                }

                return query
                    .OrderByDescending(i => i.InvoiceDate)
                    .Select(i => _mapper.Map<Invoice>(i))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public Invoice? GetElementById(string id)
        {
            try
            {
                var entity = _dbContext.Invoices
                    .Include(i => i.Customer)
                    .Include(i => i.Shipment)
                    .FirstOrDefault(i => i.Id == id);
                return entity == null ? null : _mapper.Map<Invoice>(entity);
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public Invoice? GetElementByShipmentId(string shipmentId)
        {
            try
            {
                var entity = _dbContext.Invoices
                    .Include(i => i.Customer)
                    .Include(i => i.Shipment)
                    .FirstOrDefault(i => i.ShipmentId == shipmentId);
                return entity == null ? null : _mapper.Map<Invoice>(entity);
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public void AddElement(Invoice invoice)
        {
            try
            {
                if (_dbContext.Invoices.Any(i => i.Id == invoice.Id))
                {
                    throw new ElementExistsException("Id", invoice.Id);
                }

                var shipment = _dbContext.Shipments
                    .Include(s => s.Request)
                    .ThenInclude(r => r.Customer)
                    .FirstOrDefault(s => s.Id == invoice.ShipmentId);

                if (shipment == null)
                {
                    throw new ElementNotFoundException($"Shipment {invoice.ShipmentId}");
                }

                var customerId = shipment.Request?.CustomerId;
                if (string.IsNullOrEmpty(customerId))
                {
                    throw new ElementNotFoundException($"Customer for Shipment {invoice.ShipmentId}");
                }

                var entity = new InvoiceEntity
                {
                    Id = invoice.Id,
                    ShipmentId = invoice.ShipmentId,
                    CustomerId = customerId,
                    Amount = invoice.Amount,
                    PaymentStatus = invoice.Status, // Здесь должно быть Pending, а не None
                    InvoiceDate = DateTime.UtcNow,
                    DueDate = DateTime.UtcNow.AddDays(30)
                };

                _dbContext.Invoices.Add(entity);
                _dbContext.SaveChanges();

                // Очищаем трекер
                _dbContext.ChangeTracker.Clear();
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

        public void PayInvoice(string id)
        {
            try
            {
                var entity = _dbContext.Invoices.Find(id);
                if (entity == null)
                {
                    throw new ElementNotFoundException(id);
                }

                entity.PaymentStatus = PaymentStatus.Paid;
                entity.PaidDate = DateTime.UtcNow;

                _dbContext.SaveChanges();
                _dbContext.ChangeTracker.Clear(); // Важно для свежих данных
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