using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlowGas.Database.Models
{
    [Table("Posts")]
    public class PostEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(36)]
        public string PostId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(200)]
        public string PostName { get; set; } = string.Empty;

        [Required]
        public int PostType { get; set; }

        [Required]
        [Column(TypeName = "jsonb")]
        public string ConfigurationJson { get; set; } = "{}";

        public bool IsActual { get; set; } = true;

        public DateTime ChangeDate { get; set; } = DateTime.Now;

        public DateTime ValidFrom { get; set; } = DateTime.Now;

        public DateTime? ValidTo { get; set; }

        public virtual ICollection<WorkerEntity> Workers { get; set; } = new List<WorkerEntity>();
    }
}