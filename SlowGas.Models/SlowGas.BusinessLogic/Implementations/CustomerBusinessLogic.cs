using SlowGas.Contracts.BusinessLogicsContracts;
using SlowGas.Contracts.StoragesContracts;
using Microsoft.Extensions.Logging;
using SlowGas.Models.DataModels;

namespace SlowGas.BusinessLogic.Implementations
{
    internal class CustomerBusinessLogic : ICustomerBusinessLogic
    {
        private readonly ICustomerStorageContract _customerStorage;
        private readonly ILogger _logger;

        public CustomerBusinessLogic(ICustomerStorageContract customerStorage, ILogger logger)
        {
            _customerStorage = customerStorage;
            _logger = logger;
        }

        public List<Customer> GetAllCustomers()
        {
            return new List<Customer>();
        }

        public Customer GetCustomerByData(string data)
        {
            return new Customer();
        }

        public void AddCustomer(Customer customer)
        {
        }

        public void UpdateCustomer(Customer customer)
        {
        }

        public void DeleteCustomer(string id)
        {
        }
    }
}