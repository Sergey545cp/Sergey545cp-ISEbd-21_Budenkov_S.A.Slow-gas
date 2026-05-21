using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlowGas.Database.Models
{
    [Table("Sales")]
    public class SaleEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(36)]
        public string SaleId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(36)]
        public string WorkerId { get; set; } = string.Empty;

        [MaxLength(36)]
        public string? CustomerId { get; set; }

        [Required]
        public DateTime SaleDate { get; set; }

        [Required]
        public int DiscountType { get; set; }

        public bool IsReturned { get; set; } = false;

        [Required]
        [Column(TypeName = "jsonb")]
        public string ProductsJson { get; set; } = "[]";

        [ForeignKey(nameof(WorkerId))]
        public virtual WorkerEntity Worker { get; set; }
    }
}