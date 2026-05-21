using Microsoft.EntityFrameworkCore;
using SlowGas.Database.Models;
using SlowGas.Models.DataModels;
using SlowGas.Models.Exceptions;
using SlowGas.Models.StoragesContracts;
using Newtonsoft.Json;

namespace SlowGas.Database.Implementations
{
    public class PostStorage : IPostStorageContract
    {
        private readonly SlowGasDbContext _dbContext;

        public PostStorage(SlowGasDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<PostDataModel> GetList()
        {
            try
            {
                var entities = _dbContext.Posts
                    .Where(p => p.IsActual)
                    .ToList();

                var result = new List<PostDataModel>();
                foreach (var e in entities)
                {
                    // Используем Models.Enums.PostType
                    var postType = (SlowGas.Models.Enums.PostType)e.PostType;
                    result.Add(new PostDataModel(
                        e.PostId,
                        e.PostName,
                        postType,
                        e.ConfigurationJson
                    ));
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public PostDataModel GetElementById(string id)
        {
            try
            {
                var entity = _dbContext.Posts
                    .FirstOrDefault(p => p.PostId == id && p.IsActual);

                if (entity == null)
                    return null;

                var postType = (SlowGas.Models.Enums.PostType)entity.PostType;
                return new PostDataModel(
                    entity.PostId,
                    entity.PostName,
                    postType,
                    entity.ConfigurationJson
                );
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public void AddElement(PostDataModel model)
        {
            try
            {
                model.Validate();

                var oldVersions = _dbContext.Posts.Where(p => p.PostId == model.Id && p.IsActual);
                foreach (var old in oldVersions)
                {
                    old.IsActual = false;
                    old.ValidTo = DateTime.Now;
                }

                var entity = new PostEntity
                {
                    PostId = model.Id,
                    PostName = model.PostName,
                    PostType = (int)model.PostType,
                    ConfigurationJson = JsonConvert.SerializeObject(model.ConfigurationModel),
                    IsActual = true,
                    ValidFrom = DateTime.Now,
                    ValidTo = null,
                    ChangeDate = DateTime.Now
                };

                _dbContext.Posts.Add(entity);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }

        public void UpdateElement(PostDataModel model)
        {
            AddElement(model);
        }

        public void DeleteElement(string id)
        {
            try
            {
                var entities = _dbContext.Posts.Where(p => p.PostId == id && p.IsActual);
                foreach (var entity in entities)
                {
                    entity.IsActual = false;
                    entity.ValidTo = DateTime.Now;
                }
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new StorageException(ex);
            }
        }
    }
}