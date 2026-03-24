using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlowGas.Database.Models
{
    [Table("Customers")]
    public class CustomerEntity
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        public virtual ICollection<RequestEntity> Requests { get; set; } = new List<RequestEntity>();
        public virtual ICollection<InvoiceEntity> Invoices { get; set; } = new List<InvoiceEntity>();
    }
}