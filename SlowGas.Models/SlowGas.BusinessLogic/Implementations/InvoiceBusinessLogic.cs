using SlowGas.Models.DataModels;
using SlowGas.Models.Enums;
using SlowGas.Models.Exceptions;
using SlowGas.Models.Extensions;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using SlowGas.Contracts.BusinessLogicsContracts;
using SlowGas.Contracts.StoragesContracts;

namespace SlowGas.BusinessLogic.Implementations
{
    public class InvoiceBusinessLogic : IInvoiceBusinessLogic
    {
        private readonly IInvoiceStorageContract _invoiceStorage;
        private readonly IShipmentStorageContract _shipmentStorage;
        private readonly ILogger _logger;

        public InvoiceBusinessLogic(
            IInvoiceStorageContract invoiceStorage,
            IShipmentStorageContract shipmentStorage,
            ILogger logger)
        {
            _invoiceStorage = invoiceStorage;
            _shipmentStorage = shipmentStorage;
            _logger = logger;
        }

        public List<Invoice> GetAllInvoices(DateTime? fromDate = null, DateTime? toDate = null, string? shipmentId = null)
        {
            _logger.LogInformation("GetAllInvoices called with fromDate={fromDate}, toDate={toDate}, shipmentId={shipmentId}",
                fromDate, toDate, shipmentId);

            if (fromDate.HasValue && toDate.HasValue && fromDate.Value >= toDate.Value)
                throw new IncorrectDatesException(fromDate.Value, toDate.Value);

            var result = _invoiceStorage.GetList(fromDate, toDate, shipmentId);
            if (result == null)
                throw new NullListException();

            return result;
        }

        public Invoice GetInvoiceById(string id)
        {
            _logger.LogInformation("GetInvoiceById called with id: {id}", id);

            if (id.IsEmpty())
                throw new ArgumentNullException(nameof(id));

            if (!id.IsGuid())
                throw new ValidationException("Id is not a valid GUID");

            var result = _invoiceStorage.GetElementById(id);
            if (result == null)
                throw new ElementNotFoundException(id);

            return result;
        }

        public Invoice GetInvoiceByShipmentId(string shipmentId)
        {
            _logger.LogInformation("GetInvoiceByShipmentId called with shipmentId: {shipmentId}", shipmentId);

            if (shipmentId.IsEmpty())
                throw new ArgumentNullException(nameof(shipmentId));

            if (!shipmentId.IsGuid())
                throw new ValidationException("ShipmentId is not a valid GUID");

            var result = _invoiceStorage.GetElementByShipmentId(shipmentId);
            if (result == null)
                throw new ElementNotFoundException(shipmentId);

            return result;
        }

        public void CreateInvoice(Invoice invoice)
        {
            _logger.LogInformation("CreateInvoice called: {json}", JsonSerializer.Serialize(invoice));

            ArgumentNullException.ThrowIfNull(invoice);

            var shipment = _shipmentStorage.GetElementById(invoice.ShipmentId);
            if (shipment == null)
                throw new ElementNotFoundException($"Shipment with id {invoice.ShipmentId} not found");

            if (invoice.Amount <= 0)
                throw new ValidationException("Invoice amount must be greater than 0");

            invoice.Validate();

            _invoiceStorage.AddElement(invoice);
        }

        public void PayInvoice(string id)
        {
            _logger.LogInformation("PayInvoice called with id: {id}", id);

            if (id.IsEmpty())
                throw new ArgumentNullException(nameof(id));

            if (!id.IsGuid())
                throw new ValidationException("Id is not a valid GUID");

            _invoiceStorage.PayInvoice(id);
        }
    }
}