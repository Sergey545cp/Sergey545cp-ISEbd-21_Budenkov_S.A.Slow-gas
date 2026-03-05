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
    public class ShipmentBusinessLogic : IShipmentBusinessLogic
    {
        private readonly IShipmentStorageContract _shipmentStorage;
        private readonly IRequestStorageContract _requestStorage;
        private readonly ILogger _logger;

        public ShipmentBusinessLogic(
            IShipmentStorageContract shipmentStorage,
            IRequestStorageContract requestStorage,
            ILogger logger)
        {
            _shipmentStorage = shipmentStorage;
            _requestStorage = requestStorage;
            _logger = logger;
        }

        public List<Shipment> GetAllShipments(DateTime? fromDate = null, DateTime? toDate = null, string? requestId = null)
        {
            _logger.LogInformation("GetAllShipments called with fromDate={fromDate}, toDate={toDate}, requestId={requestId}",
                fromDate, toDate, requestId);

            if (fromDate.HasValue && toDate.HasValue && fromDate.Value >= toDate.Value)
                throw new IncorrectDatesException(fromDate.Value, toDate.Value);

            var result = _shipmentStorage.GetList(fromDate, toDate, requestId);
            if (result == null)
                throw new NullListException();

            return result;
        }

        public Shipment GetShipmentById(string id)
        {
            _logger.LogInformation("GetShipmentById called with id: {id}", id);

            if (id.IsEmpty())
                throw new ArgumentNullException(nameof(id));

            if (!id.IsGuid())
                throw new ValidationException("Id is not a valid GUID");

            var result = _shipmentStorage.GetElementById(id);
            if (result == null)
                throw new ElementNotFoundException(id);

            return result;
        }

        public void CreateShipment(Shipment shipment)
        {
            _logger.LogInformation("CreateShipment called: {json}", JsonSerializer.Serialize(shipment));

            ArgumentNullException.ThrowIfNull(shipment);

            var request = _requestStorage.GetElementById(shipment.RequestId);
            if (request == null)
                throw new ElementNotFoundException($"Request with id {shipment.RequestId} not found");

            if (shipment.Date > DateTime.Now)
                throw new ValidationException("Shipment date cannot be in the future");

            shipment.Validate();

            _shipmentStorage.AddElement(shipment);
        }

        public void CancelShipment(string id)
        {
            _logger.LogInformation("CancelShipment called with id: {id}", id);

            if (id.IsEmpty())
                throw new ArgumentNullException(nameof(id));

            if (!id.IsGuid())
                throw new ValidationException("Id is not a valid GUID");

            _shipmentStorage.CancelElement(id);
        }

        public List<Shipment> CalculateMonthlyShipments(string customerId, DateTime month)
        {
            _logger.LogInformation("CalculateMonthlyShipments called for customer {customerId} in month {month}", customerId, month);

            if (customerId.IsEmpty())
                throw new ArgumentNullException(nameof(customerId));

            if (!customerId.IsGuid())
                throw new ValidationException("CustomerId is not a valid GUID");

            var startDate = new DateTime(month.Year, month.Month, 1);
            var endDate = new DateTime(month.Year, month.Month,
                DateTime.DaysInMonth(month.Year, month.Month), 23, 59, 59);

            var requests = _requestStorage.GetList(startDate, endDate, customerId);
            if (requests == null)
                throw new NullListException();

            var allShipments = new List<Shipment>();

            foreach (var request in requests)
            {
                var shipments = _shipmentStorage.GetList(null, null, request.Id);
                if (shipments != null)
                    allShipments.AddRange(shipments);
            }

            return allShipments;
        }
    }
}