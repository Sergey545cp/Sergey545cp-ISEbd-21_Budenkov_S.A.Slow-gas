using SlowGas.Models.DataModels;
using SlowGas.Models.Exceptions;
using SlowGas.Models.Extensions;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.RegularExpressions;
using SlowGas.Contracts.BusinessLogicsContracts;
using SlowGas.Contracts.StoragesContracts;

namespace SlowGas.BusinessLogic.Implementations
{
    public class CustomerBusinessLogic : ICustomerBusinessLogic
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
            _logger.LogInformation("GetAllCustomers called");
            var result = _customerStorage.GetList();
            if (result == null)
                throw new NullListException();
            return result;
        }

        public Customer GetCustomerByData(string data)
        {
            _logger.LogInformation("GetCustomerByData called with data: {data}", data);

            if (data.IsEmpty())
                throw new ArgumentNullException(nameof(data));

            if (data.IsGuid())
            {
                var result = _customerStorage.GetElementById(data);
                if (result == null)
                    throw new ElementNotFoundException(data);
                return result;
            }

            if (Regex.IsMatch(data, @"^(\+7|8)[0-9]{10}$"))
            {
                var result = _customerStorage.GetElementByPhone(data);
                if (result == null)
                    throw new ElementNotFoundException(data);
                return result;
            }

            var resultByName = _customerStorage.GetElementByName(data);
            if (resultByName == null)
                throw new ElementNotFoundException(data);

            return resultByName;
        }

        public void AddCustomer(Customer customer)
        {
            _logger.LogInformation("AddCustomer called: {json}", JsonSerializer.Serialize(customer));
            ArgumentNullException.ThrowIfNull(customer);
            customer.Validate();
            _customerStorage.AddElement(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            _logger.LogInformation("UpdateCustomer called: {json}", JsonSerializer.Serialize(customer));
            ArgumentNullException.ThrowIfNull(customer);
            customer.Validate();
            _customerStorage.UpdateElement(customer);
        }

        public void DeleteCustomer(string id)
        {
            _logger.LogInformation("DeleteCustomer called with id: {id}", id);

            if (id.IsEmpty())
                throw new ArgumentNullException(nameof(id));

            if (!id.IsGuid())
                throw new ValidationException("Id is not a valid GUID");

            _customerStorage.DeleteElement(id);
        }
    }
}