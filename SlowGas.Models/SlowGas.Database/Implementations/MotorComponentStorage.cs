using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SlowGas.Database.Models;
using SlowGas.Models.DataModels;
using SlowGas.Models.Exceptions;
using SlowGas.Models.StoragesContracts;

namespace SlowGas.Database.Implementations
{
    public class MotorComponentStorage : IMotorComponentsStorageContract
    {
        private readonly SlowGasDbContext _dbContext;
        private readonly IMapper _mapper;

        public MotorComponentStorage(SlowGasDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public List<ComponentDataModel> GetComponentsForMotor(string motorModelId)
        {
            try
            {
                return _dbContext.MotorComponents
                    .Where(mc => mc.MotorId == motorModelId)
                    .Include(mc => mc.Component)
                    .Select(mc => _mapper.Map<ComponentDataModel>(mc.Component))
                    .ToList();
            }
            catch (Exception ex)
            {
                _dbContext.ChangeTracker.Clear();
                throw new StorageException(ex);
            }
        }

        public List<MotorModelDataModel> GetMotorsForComponent(string componentId)
        {
            try
            {
                return _dbContext.MotorComponents
                    .Where(mc => mc.ComponentId == componentId)
                    .Include(mc => mc.Motor)
                    .Select(mc => _mapper.Map<MotorModelDataModel>(mc.Motor))
                    .ToList();
            }
            catch (Exception ex)
            {
                _dbContext.ChangeTracker.Clear();
                throw new StorageException(ex);
            }
        }

        public void AddComponentToMotor(MotorComponentsDataModel motorComponentsDataModel)
        {
            try
            {
                var entity = _mapper.Map<MotorComponentEntity>(motorComponentsDataModel);
                _dbContext.MotorComponents.Add(entity);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _dbContext.ChangeTracker.Clear();
                throw new StorageException(ex);
            }
        }

        public void RemoveComponentFromMotor(string motorModelId, string componentId)
        {
            try
            {
                var entity = _dbContext.MotorComponents
                    .FirstOrDefault(mc => mc.MotorId == motorModelId && mc.ComponentId == componentId)
                    ?? throw new ElementNotFoundException($"MotorComponent {motorModelId}:{componentId}");

                _dbContext.MotorComponents.Remove(entity);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _dbContext.ChangeTracker.Clear();
                throw new StorageException(ex);
            }
        }

        public void UpdateQuantity(string motorModelId, string componentId, int newQuantity)
        {
            try
            {
                var entity = _dbContext.MotorComponents
                    .FirstOrDefault(mc => mc.MotorId == motorModelId && mc.ComponentId == componentId)
                    ?? throw new ElementNotFoundException($"MotorComponent {motorModelId}:{componentId}");

                entity.QuantityRequired = newQuantity;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _dbContext.ChangeTracker.Clear();
                throw new StorageException(ex);
            }
        }
    }
}