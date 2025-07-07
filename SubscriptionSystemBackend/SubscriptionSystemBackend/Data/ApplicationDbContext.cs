using Microsoft.EntityFrameworkCore;
using SubscriptionSystemBackend.Repositories;
using SubscriptionSystemBackend.Models;

namespace SubscriptionSystemBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
                entity.Property(e => e.FiscalCode).IsRequired(false).HasMaxLength(16);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<Invitation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.InvitationToken).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.InvitationToken).IsUnique();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });
        }
    }
} 