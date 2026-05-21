using SlowGas.Models.Exceptions;
using SlowGas.Models.Extensions;

namespace SlowGas.Models.DataModels
{
    public class WorkerDataModel
    {
        public string Id { get; private set; }
        public string FullName { get; private set; }
        public string PostId { get; private set; }
        public DateTime EmploymentDate { get; private set; }
        public DateTime DateOfDelete { get; private set; }
        public bool IsDeleted { get; private set; }

        public WorkerDataModel(string id, string fullName, string postId, DateTime employmentDate, DateTime dateOfDelete, bool isDeleted)
        {
            Id = id;
            FullName = fullName;
            PostId = postId;
            EmploymentDate = employmentDate;
            DateOfDelete = dateOfDelete;
            IsDeleted = isDeleted;
        }

        public void Validate()
        {
            if (Id.IsEmpty())
                throw new ValidationException("Field Id is empty");
            if (!Id.IsGuid())
                throw new ValidationException("Id is not a valid GUID");
            if (FullName.IsEmpty())
                throw new ValidationException("Field FullName is empty");
            if (PostId.IsEmpty())
                throw new ValidationException("Field PostId is empty");
        }
    }
}