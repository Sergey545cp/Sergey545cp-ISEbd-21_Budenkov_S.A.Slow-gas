using SlowGas.Models.DataModels;

namespace SlowGas.Contracts.BusinessLogicsContracts
{
    public interface ICustomerBusinessLogic
    {
        List<Customer> GetAllCustomers();
        Customer GetCustomerByData(string data);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(string id);
    }
}