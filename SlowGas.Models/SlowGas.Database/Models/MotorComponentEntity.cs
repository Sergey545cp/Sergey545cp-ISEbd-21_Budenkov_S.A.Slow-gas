using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlowGas.Database.Models
{
    [Table("MotorComponents")]
    public class MotorComponentEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string MotorId { get; set; } = string.Empty;

        [Required]
        public string ComponentId { get; set; } = string.Empty;

        [Required]
        public int QuantityRequired { get; set; }

        public bool IsRequired { get; set; } = true;

        [ForeignKey("MotorId")]
        public virtual MotorEntity Motor { get; set; } = null!;

        [ForeignKey("ComponentId")]
        public virtual ComponentEntity Component { get; set; } = null!;
    }
}