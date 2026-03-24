using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SlowGas.Models.Enums;

namespace SlowGas.Database.Models
{
    [Table("Motors")]
    public class MotorEntity
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string ModelCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public MotorType Type { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        public virtual ICollection<MotorComponentEntity> MotorComponents { get; set; } = new List<MotorComponentEntity>();
    }
}