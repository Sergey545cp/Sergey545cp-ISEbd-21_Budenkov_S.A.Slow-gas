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
    public class MotorAdapter : IMotorAdapter
    {
        private readonly IMotorStorageContract _storage;
        private readonly IMapper _mapper;
        private readonly ILogger<MotorAdapter> _logger;

        public MotorAdapter(IMotorStorageContract storage, IMapper mapper, ILogger<MotorAdapter> logger)
        {
            _storage = storage;
            _mapper = mapper;
            _logger = logger;
        }

        public MotorOperationResponse GetList(bool onlyActive = true)
        {
            try
            {
                var motors = _storage.GetList(onlyActive);
                var viewModels = _mapper.Map<List<MotorViewModel>>(motors);
                return MotorOperationResponse.OK(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting motors");
                return MotorOperationResponse.InternalServerError(ex.Message);
            }
        }

        public MotorOperationResponse GetElement(string data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data))
                    return MotorOperationResponse.BadRequest("Data is empty");

                Motor? motor = null;
                if (Guid.TryParse(data, out _))
                    motor = _storage.GetElementById(data);
                else
                {
                    motor = _storage.GetElementByModelCode(data);
                    if (motor == null)
                        motor = _storage.GetElementByName(data);
                }

                if (motor == null)
                    return MotorOperationResponse.NotFound($"Motor not found: {data}");

                return MotorOperationResponse.OK(_mapper.Map<MotorViewModel>(motor));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting motor");
                return MotorOperationResponse.InternalServerError(ex.Message);
            }
        }

        public MotorOperationResponse Create(MotorBindingModel model)
        {
            try
            {
                if (model == null)
                    return MotorOperationResponse.BadRequest("Model is null");

                var motor = _mapper.Map<Motor>(model);
                motor.Id = Guid.NewGuid().ToString();
                _storage.AddElement(motor);
                return MotorOperationResponse.NoContent();
            }
            catch (ElementExistsException ex)
            {
                return MotorOperationResponse.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating motor");
                return MotorOperationResponse.InternalServerError(ex.Message);
            }
        }

        public MotorOperationResponse Update(MotorBindingModel model)
        {
            try
            {
                if (model == null)
                    return MotorOperationResponse.BadRequest("Model is null");

                if (string.IsNullOrWhiteSpace(model.Id))
                    return MotorOperationResponse.BadRequest("Id is required");

                var motor = _mapper.Map<Motor>(model);
                _storage.UpdateElement(motor);
                return MotorOperationResponse.NoContent();
            }
            catch (ElementNotFoundException ex)
            {
                return MotorOperationResponse.NotFound(ex.Message);
            }
            catch (ElementExistsException ex)
            {
                return MotorOperationResponse.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating motor");
                return MotorOperationResponse.InternalServerError(ex.Message);
            }
        }

        public MotorOperationResponse Delete(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return MotorOperationResponse.BadRequest("Id is empty");

                _storage.DeleteElement(id);
                return MotorOperationResponse.NoContent();
            }
            catch (ElementNotFoundException ex)
            {
                return MotorOperationResponse.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting motor");
                return MotorOperationResponse.InternalServerError(ex.Message);
            }
        }
    }

    public interface IMotorAdapter
    {
        MotorOperationResponse GetList(bool onlyActive = true);
        MotorOperationResponse GetElement(string data);
        MotorOperationResponse Create(MotorBindingModel model);
        MotorOperationResponse Update(MotorBindingModel model);
        MotorOperationResponse Delete(string id);
    }
}