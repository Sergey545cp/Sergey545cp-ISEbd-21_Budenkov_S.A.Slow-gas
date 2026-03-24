using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SlowGas.Database.Models;
using SlowGas.Models.DataModels;
using SlowGas.Models.Exceptions;
using SlowGas.Models.StoragesContracts;
using Npgsql;
using SlowGas.Contracts.StoragesContracts;

namespace SlowGas.Database.Implementations
{
    public class CustomerStorage : ICustomerStorageContract
    {
        private readonly SlowGasDbContext _dbContext;
        private readonly IMapper _mapper;

        public CustomerStorage(SlowGasDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public List<Customer> GetList()
        {
            try
            {
                return _dbContext.Customers
                    .Select(c => _mapper.Map<Customer>(c))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public Customer? GetElementById(string id)
        {
            try
            {
                var entity = _dbContext.Customers.Find(id);
                return entity == null ? null : _mapper.Map<Customer>(entity);
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public Customer? GetElementByName(string name)
        {
            try
            {
                var entity = _dbContext.Customers
                    .FirstOrDefault(c => c.Name == name);
                return entity == null ? null : _mapper.Map<Customer>(entity);
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public Customer? GetElementByPhone(string phone)
        {
            try
            {
                var entity = _dbContext.Customers
                    .FirstOrDefault(c => c.Phone == phone);
                return entity == null ? null : _mapper.Map<Customer>(entity);
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public void AddElement(Customer customer)
        {
            try
            {
                if (_dbContext.Customers.Any(c => c.Id == customer.Id))
                {
                    throw new ElementExistsException("Id", customer.Id);
                }

                if (_dbContext.Customers.Any(c => c.Phone == customer.Phone))
                {
                    throw new ElementExistsException("Phone", customer.Phone);
                }

                var entity = _mapper.Map<CustomerEntity>(customer);
                _dbContext.Customers.Add(entity);
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx
                && pgEx.ConstraintName == "IX_Customers_Phone")
            {
                throw new ElementExistsException("Phone", customer.Phone);
            }
            catch (ElementExistsException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public void UpdateElement(Customer customer)
        {
            try
            {
                var entity = _dbContext.Customers.Find(customer.Id);
                if (entity == null)
                {
                    throw new ElementNotFoundException(customer.Id);
                }

                if (entity.Phone != customer.Phone &&
                    _dbContext.Customers.Any(c => c.Phone == customer.Phone && c.Id != customer.Id))
                {
                    throw new ElementExistsException("Phone", customer.Phone);
                }

                _mapper.Map(customer, entity);
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx
                && pgEx.ConstraintName == "IX_Customers_Phone")
            {
                throw new ElementExistsException("Phone", customer.Phone);
            }
            catch (ElementNotFoundException)
            {
                throw;
            }
            catch (ElementExistsException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public void DeleteElement(string id)
        {
            try
            {
                var entity = _dbContext.Customers.Find(id);
                if (entity == null)
                {
                    throw new ElementNotFoundException(id);
                }

                _dbContext.Customers.Remove(entity);
                _dbContext.SaveChanges();
            }
            catch (ElementNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }
    }
}