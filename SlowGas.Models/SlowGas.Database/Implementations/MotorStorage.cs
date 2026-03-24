using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SlowGas.Contracts.StoragesContracts;
using SlowGas.Database.Models;
using SlowGas.Models.DataModels;
using SlowGas.Models.Enums;
using SlowGas.Models.Exceptions;
using SlowGas.Models.StoragesContracts;

namespace SlowGas.Database.Implementations
{
    public class MotorStorage : IMotorStorageContract
    {
        private readonly SlowGasDbContext _dbContext;
        private readonly IMapper _mapper;

        public MotorStorage(SlowGasDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public List<Motor> GetList(bool onlyActive = true)
        {
            try
            {
                var query = _dbContext.Motors.AsQueryable();
                if (onlyActive)
                {
                    query = query.Where(m => m.IsActive);
                }
                return query.Select(m => _mapper.Map<Motor>(m)).ToList();
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public Motor? GetElementById(string id)
        {
            try
            {
                // Добавляем фильтр по IsActive
                var entity = _dbContext.Motors.FirstOrDefault(m => m.Id == id && m.IsActive);
                return entity == null ? null : _mapper.Map<Motor>(entity);
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public Motor? GetElementByName(string name)
        {
            try
            {
                var entity = _dbContext.Motors.FirstOrDefault(m => m.Name == name && m.IsActive);
                return entity == null ? null : _mapper.Map<Motor>(entity);
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public Motor? GetElementByModelCode(string modelCode)
        {
            try
            {
                var entity = _dbContext.Motors.FirstOrDefault(m => m.ModelCode == modelCode && m.IsActive);
                return entity == null ? null : _mapper.Map<Motor>(entity);
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public void AddElement(Motor motor)
        {
            try
            {
                if (_dbContext.Motors.Any(m => m.Id == motor.Id))
                    throw new ElementExistsException("Id", motor.Id);
                if (_dbContext.Motors.Any(m => m.ModelCode == motor.ModelCode))
                    throw new ElementExistsException("ModelCode", motor.ModelCode);

                var entity = _mapper.Map<MotorEntity>(motor);
                entity.ValidFrom = DateTime.UtcNow;
                entity.ValidTo = null;
                entity.IsActive = true;
                _dbContext.Motors.Add(entity);
                _dbContext.SaveChanges();
            }
            catch (ElementExistsException) { throw; }
            catch (Exception ex) { throw new StorageException(ex); }
        }

        public void UpdateElement(Motor motor)
        {
            try
            {
                // Получаем старую сущность без отслеживания
                var entity = _dbContext.Motors.AsNoTracking().FirstOrDefault(m => m.Id == motor.Id);
                if (entity == null) throw new ElementNotFoundException(motor.Id);

                if (entity.ModelCode != motor.ModelCode &&
                    _dbContext.Motors.Any(m => m.ModelCode == motor.ModelCode && m.Id != motor.Id))
                    throw new ElementExistsException("ModelCode", motor.ModelCode);

                if (entity.Price != motor.Price)
                {
                    // Деактивируем старую запись
                    var oldEntity = _dbContext.Motors.Find(motor.Id);
                    oldEntity.ValidTo = DateTime.UtcNow;
                    oldEntity.IsActive = false;
                    _dbContext.Entry(oldEntity).State = EntityState.Modified;

                    // Создаем новую запись с тем же ID и ModelCode
                    var newEntity = new MotorEntity
                    {
                        Id = motor.Id, // Используем тот же ID
                        ModelCode = entity.ModelCode, // Сохраняем тот же ModelCode
                        Name = motor.Name,
                        Type = motor.Type,
                        Price = motor.Price,
                        ValidFrom = DateTime.UtcNow,
                        ValidTo = null,
                        IsActive = true
                    };

                    _dbContext.Motors.Add(newEntity);
                }
                else
                {
                    var existingEntity = _dbContext.Motors.Find(motor.Id);
                    _mapper.Map(motor, existingEntity);
                    _dbContext.Entry(existingEntity).State = EntityState.Modified;
                }

                _dbContext.SaveChanges();
                _dbContext.ChangeTracker.Clear();
            }
            catch (ElementNotFoundException) { throw; }
            catch (ElementExistsException) { throw; }
            catch (Exception ex) { throw new StorageException(ex); }
        }

        public void DeleteElement(string id)
        {
            try
            {
                var entity = _dbContext.Motors.Find(id);
                if (entity == null) throw new ElementNotFoundException(id);
                entity.IsActive = false;
                entity.ValidTo = DateTime.UtcNow;
                _dbContext.SaveChanges();
                _dbContext.ChangeTracker.Clear();
            }
            catch (ElementNotFoundException) { throw; }
            catch (Exception ex) { throw new StorageException(ex); }
        }
    }
}