using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlowGas.Database.Models
{
    [Table("Workers")]
    public class WorkerEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(36)]
        public string WorkerId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(36)]
        public string PostId { get; set; } = string.Empty;

        [Required]
        public DateTime EmploymentDate { get; set; }

        public DateTime? DateOfDelete { get; set; }

        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(PostId))]
        public virtual PostEntity Post { get; set; }

        public virtual ICollection<SaleEntity> Sales { get; set; } = new List<SaleEntity>();

        public virtual ICollection<SalaryEntity> Salaries { get; set; } = new List<SalaryEntity>();
    }
}