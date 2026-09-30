using Microsoft.EntityFrameworkCore;
using IncomeTaxManagementSystem.Models;

namespace IncomeTaxManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<TaxDetails> TaxDetails { get; set; } = null!;
        public DbSet<Document> Documents { get; set; } = null!;
        public DbSet<Application> Applications { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Entity Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.PAN).IsUnique();

                entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PAN).IsRequired().HasMaxLength(10);
                entity.Property(u => u.Mobile).IsRequired().HasMaxLength(15);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(u => u.Address).HasMaxLength(500);
                entity.Property(u => u.Role).IsRequired().HasMaxLength(50).HasDefaultValue("User");
                entity.Property(u => u.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Active");
                entity.Property(u => u.CreatedDate).HasDefaultValueSql("GETDATE()");
            });

            // TaxDetails Entity Configuration & Relationship
            modelBuilder.Entity<TaxDetails>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.AssessmentYear).IsRequired().HasMaxLength(20);
                entity.Property(t => t.EmploymentType).IsRequired().HasMaxLength(50);
                entity.Property(t => t.EmployerName).HasMaxLength(150);
                entity.Property(t => t.GrossSalary).HasPrecision(18, 2);
                entity.Property(t => t.OtherIncome).HasPrecision(18, 2);
                entity.Property(t => t.TDS).HasPrecision(18, 2);
                entity.Property(t => t.Deductions).HasPrecision(18, 2);
                entity.Property(t => t.TaxRegime).IsRequired().HasMaxLength(20);
                entity.Property(t => t.Remarks).HasMaxLength(500);
                entity.Property(t => t.CreatedDate).HasDefaultValueSql("GETDATE()");

                // Relationship: User 1 -> Many TaxDetails with Restrict delete behavior
                entity.HasOne(t => t.User)
                      .WithMany(u => u.TaxDetails)
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Document Entity Configuration & Relationship
            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.Property(d => d.DocumentType).IsRequired().HasMaxLength(100);
                entity.Property(d => d.FileName).IsRequired().HasMaxLength(255);
                entity.Property(d => d.FilePath).IsRequired().HasMaxLength(500);
                entity.Property(d => d.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");
                entity.Property(d => d.UploadedDate).HasDefaultValueSql("GETDATE()");

                // Relationship: User 1 -> Many Documents with Restrict delete behavior
                entity.HasOne(d => d.User)
                      .WithMany(u => u.Documents)
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Application Entity Configuration & Relationship
            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.HasIndex(a => a.ApplicationNumber).IsUnique();

                entity.Property(a => a.ApplicationNumber).IsRequired().HasMaxLength(50);
                entity.Property(a => a.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Draft");
                entity.Property(a => a.AdminRemarks).HasMaxLength(500);

                // Relationship: User 1 -> Many Applications with Restrict delete behavior
                entity.HasOne(a => a.User)
                      .WithMany(u => u.Applications)
                      .HasForeignKey(a => a.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
