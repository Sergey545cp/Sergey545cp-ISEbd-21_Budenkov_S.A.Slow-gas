using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlowGas.Database.Models
{
    [Table("Salaries")]
    public class SalaryEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(36)]
        public string SalaryId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(36)]
        public string WorkerId { get; set; } = string.Empty;

        [Required]
        public DateTime Period { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime CalculationDate { get; set; } = DateTime.Now;

        [ForeignKey(nameof(WorkerId))]
        public virtual WorkerEntity Worker { get; set; }
    }
}