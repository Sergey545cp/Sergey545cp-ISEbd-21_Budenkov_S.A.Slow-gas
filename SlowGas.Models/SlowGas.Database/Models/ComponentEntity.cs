using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlowGas.Database.Models
{
    [Table("Components")]
    public class ComponentEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(50)]
        public string ComponentCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Specification { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        public virtual ICollection<MotorComponentEntity> MotorComponents { get; set; } = new List<MotorComponentEntity>();
    }
}