using SlowGas.Models.Enums;
using SlowGas.Models.Exceptions;
using SlowGas.Models.Extensions;
using SlowGas.Models.Infrastructure;
using SlowGas.Models.Infrastructure.PostConfigurations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SlowGas.Models.DataModels
{
    public class PostDataModel : IValidation
    {
        public string Id { get; private set; }
        public string PostName { get; private set; }
        public PostType PostType { get; private set; }
        public PostConfiguration ConfigurationModel { get; private set; }

        public PostDataModel(string postId, string postName, PostType postType, PostConfiguration configuration)
        {
            Id = postId;
            PostName = postName;
            PostType = postType;
            ConfigurationModel = configuration ?? new PostConfiguration();
        }

        public PostDataModel(string postId, string postName, PostType postType, string configurationJson)
        {
            Id = postId;
            PostName = postName;
            PostType = postType;

            if (!string.IsNullOrWhiteSpace(configurationJson))
            {
                try
                {
                    var obj = JToken.Parse(configurationJson);
                    var type = obj.Value<string>("Type");

                    ConfigurationModel = type switch
                    {
                        nameof(CashierPostConfiguration) => JsonConvert.DeserializeObject<CashierPostConfiguration>(configurationJson),
                        nameof(SupervisorPostConfiguration) => JsonConvert.DeserializeObject<SupervisorPostConfiguration>(configurationJson),
                        _ => JsonConvert.DeserializeObject<PostConfiguration>(configurationJson) ?? new PostConfiguration()
                    };
                }
                catch
                {
                    ConfigurationModel = new PostConfiguration();
                }
            }
            else
            {
                ConfigurationModel = new PostConfiguration();
            }

            
            if (ConfigurationModel == null)
            {
                ConfigurationModel = new PostConfiguration();
            }
        }

        public void Validate()
        {
            if (Id.IsEmpty())
                throw new ValidationException("Field Id is empty");
            if (!Id.IsGuid())
                throw new ValidationException("Id is not a valid GUID");
            if (PostName.IsEmpty())
                throw new ValidationException("Field PostName is empty");
            if (PostType == PostType.None)
                throw new ValidationException("Field PostType is empty");
            if (ConfigurationModel == null)
                throw new ValidationException("ConfigurationModel is not initialized");
            if (ConfigurationModel.Rate <= 0)
                throw new ValidationException("Rate must be greater than 0");
        }
    }
}