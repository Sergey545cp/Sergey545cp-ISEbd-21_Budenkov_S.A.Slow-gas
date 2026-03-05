using SlowGas.Models.DataModels;

namespace SlowGas.Contracts.StoragesContracts
{
    public interface ICustomerStorageContract
    {
        List<Customer> GetList();
        Customer? GetElementById(string id);
        Customer? GetElementByName(string name);
        Customer? GetElementByPhone(string phone);
        void AddElement(Customer customer);
        void UpdateElement(Customer customer);
        void DeleteElement(string id);
    }
}