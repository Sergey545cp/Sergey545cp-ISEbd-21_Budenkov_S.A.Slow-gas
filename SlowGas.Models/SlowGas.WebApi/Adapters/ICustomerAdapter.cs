using SlowGas.WebApi.Infrastructure;
using SlowGas.Models.BindingModels;

namespace SlowGas.WebApi.Adapters
{
    public interface ICustomerAdapter
    {
        CustomerOperationResponse GetList();
        CustomerOperationResponse GetElement(string data);
        CustomerOperationResponse Create(CustomerBindingModel model);
        CustomerOperationResponse Update(CustomerBindingModel model);
        CustomerOperationResponse Delete(string id);
    }
}