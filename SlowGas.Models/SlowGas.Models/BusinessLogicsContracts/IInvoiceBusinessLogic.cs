using SlowGas.Models.DataModels;

namespace SlowGas.Contracts.BusinessLogicsContracts
{
    public interface IInvoiceBusinessLogic
    {
        List<Invoice> GetAllInvoices(DateTime? fromDate = null, DateTime? toDate = null, string? shipmentId = null);
        Invoice GetInvoiceById(string id);
        Invoice GetInvoiceByShipmentId(string shipmentId);
        void CreateInvoice(Invoice invoice);
        void PayInvoice(string id);
    }
}