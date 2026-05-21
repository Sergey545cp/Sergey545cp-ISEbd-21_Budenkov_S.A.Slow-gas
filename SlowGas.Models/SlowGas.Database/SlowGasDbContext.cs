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

        // НОВЫЕ DbSet
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<WorkerEntity> Workers { get; set; }
        public DbSet<SaleEntity> Sales { get; set; }
        public DbSet<SalaryEntity> Salaries { get; set; }

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

            // Существующие настройки
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

            // НОВЫЕ НАСТРОЙКИ
            modelBuilder.Entity<PostEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PostId).IsRequired().HasMaxLength(36);
                entity.HasIndex(e => e.PostId).IsUnique();
                entity.Property(e => e.PostName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ConfigurationJson).HasColumnType("jsonb").IsRequired();
                entity.HasIndex(e => e.ConfigurationJson).HasMethod("GIN");

                entity.HasMany(e => e.Workers)
                    .WithOne(w => w.Post)
                    .HasForeignKey(w => w.PostId)
                    .HasPrincipalKey(e => e.PostId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<WorkerEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.WorkerId).IsRequired().HasMaxLength(36);
                entity.HasIndex(e => e.WorkerId).IsUnique();
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.PostId).IsRequired().HasMaxLength(36);
                entity.HasIndex(e => e.EmploymentDate);

                entity.HasMany(e => e.Sales)
                    .WithOne(s => s.Worker)
                    .HasForeignKey(s => s.WorkerId)
                    .HasPrincipalKey(e => e.WorkerId);

                entity.HasMany(e => e.Salaries)
                    .WithOne(s => s.Worker)
                    .HasForeignKey(s => s.WorkerId)
                    .HasPrincipalKey(e => e.WorkerId);
            });

            modelBuilder.Entity<SaleEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SaleId).IsRequired().HasMaxLength(36);
                entity.HasIndex(e => e.SaleId).IsUnique();
                entity.Property(e => e.ProductsJson).HasColumnType("jsonb").IsRequired();
                entity.HasIndex(e => e.SaleDate);
                entity.HasIndex(e => e.WorkerId);
            });

            modelBuilder.Entity<SalaryEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SalaryId).IsRequired().HasMaxLength(36);
                entity.HasIndex(e => e.SalaryId).IsUnique();
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                entity.HasIndex(e => e.WorkerId);
                entity.HasIndex(e => e.Period);
                entity.HasIndex(e => new { e.WorkerId, e.Period }).IsUnique();
            });
        }
    }
}