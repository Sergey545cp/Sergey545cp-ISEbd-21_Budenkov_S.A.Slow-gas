using SlowGas.Models.DataModels;

namespace SlowGas.Contracts.StoragesContracts
{
    public interface IShipmentStorageContract
    {
        List<Shipment> GetList(DateTime? fromDate = null, DateTime? toDate = null, string? requestId = null);
        Shipment? GetElementById(string id);
        void AddElement(Shipment shipment);
        void CancelElement(string id);
    }
}