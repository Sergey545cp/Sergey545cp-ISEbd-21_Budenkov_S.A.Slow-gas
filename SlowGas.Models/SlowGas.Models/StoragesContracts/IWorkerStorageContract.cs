using SlowGas.Models.DataModels;

namespace SlowGas.Models.StoragesContracts
{
    public interface IWorkerStorageContract
    {
        List<WorkerDataModel> GetList();
        WorkerDataModel GetElementById(string id);
        void AddElement(WorkerDataModel model);
        void UpdateElement(WorkerDataModel model);
        void DeleteElement(string id);

        // НОВЫЙ МЕТОД
        int GetWorkerTrend(DateTime fromPeriod, DateTime toPeriod);
    }
}