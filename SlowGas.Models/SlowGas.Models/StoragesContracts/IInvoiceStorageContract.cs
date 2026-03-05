using SlowGas.Models.DataModels;

namespace SlowGas.Contracts.StoragesContracts
{
    public interface IInvoiceStorageContract
    {
        List<Invoice> GetList(DateTime? fromDate = null, DateTime? toDate = null, string? shipmentId = null);
        Invoice? GetElementById(string id);
        Invoice? GetElementByShipmentId(string shipmentId);
        void AddElement(Invoice invoice);
        void PayInvoice(string id);
    }
}