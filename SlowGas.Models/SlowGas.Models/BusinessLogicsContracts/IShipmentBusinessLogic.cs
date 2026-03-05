using SlowGas.Models.DataModels;

namespace SlowGas.Contracts.BusinessLogicsContracts
{
    public interface IShipmentBusinessLogic
    {
        List<Shipment> GetAllShipments(DateTime? fromDate = null, DateTime? toDate = null, string? requestId = null);
        Shipment GetShipmentById(string id);
        void CreateShipment(Shipment shipment);
        void CancelShipment(string id);
        List<Shipment> CalculateMonthlyShipments(string customerId, DateTime month);
    }
}