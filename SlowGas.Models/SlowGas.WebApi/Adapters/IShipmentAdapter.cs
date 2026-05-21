using SlowGas.Models.BindingModels;
using SlowGas.WebApi.Infrastructure;

namespace SlowGas.WebApi.Adapters
{
    public interface IShipmentAdapter
    {
        ShipmentOperationResponse GetList(DateTime? fromDate, DateTime? toDate, string? requestId);
        ShipmentOperationResponse GetElement(string id);
        ShipmentOperationResponse Create(ShipmentBindingModel model);
        ShipmentOperationResponse Cancel(string id);
    }
}