using SlowGas.Models.DataModels;

namespace SlowGas.Models.StoragesContracts
{
    public interface IPostStorageContract
    {
        List<PostDataModel> GetList();
        PostDataModel GetElementById(string id);
        void AddElement(PostDataModel model);
        void UpdateElement(PostDataModel model);
        void DeleteElement(string id);
    }
}