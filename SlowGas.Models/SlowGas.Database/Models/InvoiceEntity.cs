using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SlowGas.Models.Enums;

namespace SlowGas.Database.Models
{
    [Table("Invoices")]
    public class InvoiceEntity
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [Required]
        public string ShipmentId { get; set; } = string.Empty;

        [Required]
        public string CustomerId { get; set; } = string.Empty;

        [Required]
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(30);

        [Required]
        public PaymentStatus PaymentStatus { get; set; }

        [ForeignKey("ShipmentId")]
        public virtual ShipmentEntity Shipment { get; set; } = null!;

        [ForeignKey("CustomerId")]
        public virtual CustomerEntity Customer { get; set; } = null!;
        public DateTime? PaidDate { get; set; }

    }
}