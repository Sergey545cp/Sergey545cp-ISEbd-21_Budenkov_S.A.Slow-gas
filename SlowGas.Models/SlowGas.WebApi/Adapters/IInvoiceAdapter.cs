using SlowGas.Models.BindingModels;
using SlowGas.WebApi.Infrastructure;

namespace SlowGas.WebApi.Adapters
{
    public interface IInvoiceAdapter
    {
        InvoiceOperationResponse GetList(DateTime? fromDate, DateTime? toDate, string? shipmentId);
        InvoiceOperationResponse GetElement(string id);
        InvoiceOperationResponse GetByShipmentId(string shipmentId);
        InvoiceOperationResponse Create(InvoiceBindingModel model);
        InvoiceOperationResponse Pay(string id);
    }
}