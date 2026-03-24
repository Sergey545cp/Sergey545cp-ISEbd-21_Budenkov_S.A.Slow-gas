using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SlowGas.Models.Enums;

namespace SlowGas.Database.Models
{
    [Table("Requests")]
    public class RequestEntity
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [Required]
        public string CustomerId { get; set; } = string.Empty;

        [Required]
        public DateTime RequestDate { get; set; }

        public DateTime? RequiredDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        // Убираем TotalAmount, так как его нет в модели

        [ForeignKey("CustomerId")]
        public virtual CustomerEntity Customer { get; set; } = null!;

        public virtual ICollection<ShipmentEntity> Shipments { get; set; } = new List<ShipmentEntity>();
    }
}