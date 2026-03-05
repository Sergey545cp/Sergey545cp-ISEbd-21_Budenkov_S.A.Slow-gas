using SlowGas.Contracts.BusinessLogicsContracts;

using SlowGas.Contracts.StoragesContracts;
using Microsoft.Extensions.Logging;
using SlowGas.Models.DataModels;

namespace SlowGas.BusinessLogic.Implementations
{
    internal class ShipmentBusinessLogic : IShipmentBusinessLogic
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
            return new List<Shipment>();
        }

        public Shipment GetShipmentById(string id)
        {
            return new Shipment();
        }

        public void CreateShipment(Shipment shipment)
        {
        }

        public void CancelShipment(string id)
        {
        }

        public List<Shipment> CalculateMonthlyShipments(string customerId, DateTime month)
        {
            return new List<Shipment>();
        }
    }
}