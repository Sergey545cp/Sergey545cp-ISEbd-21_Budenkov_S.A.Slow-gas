using AutoMapper;
using SlowGas.Contracts.StoragesContracts;
using SlowGas.Models.BindingModels;
using SlowGas.Models.DataModels;
using SlowGas.Models.Exceptions;
using SlowGas.Models.StoragesContracts;
using SlowGas.Models.ViewModels;
using SlowGas.WebApi.Infrastructure;

namespace SlowGas.WebApi.Adapters
{
    public class CustomerAdapter : ICustomerAdapter
    {
        private readonly ICustomerStorageContract _storage;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerAdapter> _logger;

        public CustomerAdapter(ICustomerStorageContract storage, IMapper mapper, ILogger<CustomerAdapter> logger)
        {
            _storage = storage;
            _mapper = mapper;
            _logger = logger;
        }

        public CustomerOperationResponse GetList()
        {
            try
            {
                var customers = _storage.GetList();
                var viewModels = _mapper.Map<List<CustomerViewModel>>(customers);
                return CustomerOperationResponse.OK(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customers");
                return CustomerOperationResponse.InternalServerError(ex.Message);
            }
        }

        public CustomerOperationResponse GetElement(string data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data))
                    return CustomerOperationResponse.BadRequest("Data is empty");

                Customer? customer = null;
                if (Guid.TryParse(data, out _))
                    customer = _storage.GetElementById(data);
                else if (data.StartsWith("+") || data.StartsWith("8"))
                    customer = _storage.GetElementByPhone(data);
                else
                    customer = _storage.GetElementByName(data);

                if (customer == null)
                    return CustomerOperationResponse.NotFound($"Customer not found: {data}");

                return CustomerOperationResponse.OK(_mapper.Map<CustomerViewModel>(customer));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer");
                return CustomerOperationResponse.InternalServerError(ex.Message);
            }
        }

        public CustomerOperationResponse Create(CustomerBindingModel model)
        {
            try
            {
                if (model == null)
                    return CustomerOperationResponse.BadRequest("Model is null");

                var customer = _mapper.Map<Customer>(model);
                customer.Id = Guid.NewGuid().ToString();
                _storage.AddElement(customer);
                return CustomerOperationResponse.NoContent();
            }
            catch (ElementExistsException ex)
            {
                return CustomerOperationResponse.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating customer");
                return CustomerOperationResponse.InternalServerError(ex.Message);
            }
        }

        public CustomerOperationResponse Update(CustomerBindingModel model)
        {
            try
            {
                if (model == null)
                    return CustomerOperationResponse.BadRequest("Model is null");

                if (string.IsNullOrWhiteSpace(model.Id))
                    return CustomerOperationResponse.BadRequest("Id is required");

                var customer = _mapper.Map<Customer>(model);
                _storage.UpdateElement(customer);
                return CustomerOperationResponse.NoContent();
            }
            catch (ElementNotFoundException ex)
            {
                return CustomerOperationResponse.NotFound(ex.Message);
            }
            catch (ElementExistsException ex)
            {
                return CustomerOperationResponse.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer");
                return CustomerOperationResponse.InternalServerError(ex.Message);
            }
        }

        public CustomerOperationResponse Delete(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return CustomerOperationResponse.BadRequest("Id is empty");

                _storage.DeleteElement(id);
                return CustomerOperationResponse.NoContent();
            }
            catch (ElementNotFoundException ex)
            {
                return CustomerOperationResponse.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting customer");
                return CustomerOperationResponse.InternalServerError(ex.Message);
            }
        }
    }
}