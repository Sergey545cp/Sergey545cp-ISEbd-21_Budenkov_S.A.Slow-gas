using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SlowGas.Models.Enums;

namespace SlowGas.Database.Models
{
    [Table("Shipments")]
    public class ShipmentEntity
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [Required]
        public string RequestId { get; set; } = string.Empty;

        [Required]
        public DateTime ShipmentDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        public DateTime VersionDate { get; set; }

        [ForeignKey("RequestId")]
        public virtual RequestEntity Request { get; set; } = null!;

        public virtual InvoiceEntity Invoice { get; set; } = null!;
    }
}