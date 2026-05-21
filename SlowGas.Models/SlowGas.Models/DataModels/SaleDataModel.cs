using SlowGas.Models.Enums;
using SlowGas.Models.Exceptions;
using SlowGas.Models.Extensions;

namespace SlowGas.Models.DataModels
{
    public class SaleDataModel
    {
        public string Id { get; private set; }
        public string WorkerId { get; private set; }
        public string? CustomerId { get; private set; }
        public DateTime SaleDate { get; private set; }
        public DiscountType DiscountType { get; private set; }
        public bool IsReturned { get; private set; }
        public List<SaleProductDataModel> Products { get; private set; }
        public double Sum => Products?.Sum(p => p.TotalPrice) ?? 0;

        public SaleDataModel(string id, string workerId, string? customerId, DiscountType discountType, bool isReturned, List<SaleProductDataModel> products)
        {
            Id = id;
            WorkerId = workerId;
            CustomerId = customerId;
            SaleDate = DateTime.Now;
            DiscountType = discountType;
            IsReturned = isReturned;
            Products = products ?? new List<SaleProductDataModel>();
        }

        public void Validate()
        {
            if (Id.IsEmpty())
                throw new ValidationException("Field Id is empty");
            if (!Id.IsGuid())
                throw new ValidationException("Id is not a valid GUID");
            if (WorkerId.IsEmpty())
                throw new ValidationException("Field WorkerId is empty");
            if (!WorkerId.IsGuid())
                throw new ValidationException("WorkerId is not a valid GUID");
        }
    }

    public class SaleProductDataModel
    {
        public string Id { get; private set; }
        public string ProductId { get; private set; }
        public int Quantity { get; private set; }
        public double Price { get; private set; }
        public double TotalPrice => Quantity * Price;

        public SaleProductDataModel(string id, string productId, int quantity, double price)
        {
            Id = id;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }
    }
}