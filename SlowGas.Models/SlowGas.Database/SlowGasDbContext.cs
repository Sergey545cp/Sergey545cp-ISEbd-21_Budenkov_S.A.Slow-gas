using Microsoft.EntityFrameworkCore;
using SlowGas.Database.Models;
using SlowGas.Models.Infrastructure;

namespace SlowGas.Database
{
    public class SlowGasDbContext : DbContext
    {
        private readonly IConfigurationDatabase? _configuration;

        public SlowGasDbContext(IConfigurationDatabase configuration)
        {
            _configuration = configuration;
        }

        public SlowGasDbContext(DbContextOptions<SlowGasDbContext> options)
            : base(options)
        {
        }

        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<MotorEntity> Motors { get; set; }
        public DbSet<ComponentEntity> Components { get; set; }
        public DbSet<MotorComponentEntity> MotorComponents { get; set; }
        public DbSet<RequestEntity> Requests { get; set; }
        public DbSet<ShipmentEntity> Shipments { get; set; }
        public DbSet<InvoiceEntity> Invoices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured && _configuration != null)
            {
                optionsBuilder.UseNpgsql(_configuration.ConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomerEntity>()
                .HasIndex(c => c.Phone)
                .IsUnique();

            modelBuilder.Entity<MotorEntity>()
                .HasIndex(m => m.ModelCode)
                .IsUnique();

            modelBuilder.Entity<ComponentEntity>()
                .HasIndex(c => c.ComponentCode)
                .IsUnique();

            modelBuilder.Entity<MotorComponentEntity>()
                .HasIndex(mc => new { mc.MotorId, mc.ComponentId })
                .IsUnique();

            modelBuilder.Entity<RequestEntity>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Requests)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShipmentEntity>()
                .HasOne(s => s.Request)
                .WithMany(r => r.Shipments)
                .HasForeignKey(s => s.RequestId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShipmentEntity>()
                .HasOne(s => s.Invoice)
                .WithOne(i => i.Shipment)
                .HasForeignKey<InvoiceEntity>(i => i.ShipmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InvoiceEntity>()
                .HasOne(i => i.Customer)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MotorComponentEntity>()
                .HasOne(mc => mc.Motor)
                .WithMany(m => m.MotorComponents)
                .HasForeignKey(mc => mc.MotorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MotorComponentEntity>()
                .HasOne(mc => mc.Component)
                .WithMany(c => c.MotorComponents)
                .HasForeignKey(mc => mc.ComponentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RequestEntity>()
                .HasIndex(r => r.RequestDate);

            modelBuilder.Entity<ShipmentEntity>()
                .HasIndex(s => s.ShipmentDate);

            modelBuilder.Entity<InvoiceEntity>()
                .HasIndex(i => i.InvoiceDate);
        }
    }
}