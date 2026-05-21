using SlowGas.Models.BindingModels;
using SlowGas.WebApi.Infrastructure;

namespace SlowGas.WebApi.Adapters
{
    public interface IRequestAdapter
    {
        RequestOperationResponse GetList(DateTime? fromDate, DateTime? toDate, string? customerId);
        RequestOperationResponse GetElement(string id);
        RequestOperationResponse Create(RequestBindingModel model);
        RequestOperationResponse Cancel(string id);
    }
}