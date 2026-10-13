using System.Drawing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimeTracker.Models;

namespace TimeTracker.Data;

public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext(options) {
    public DbSet<Step> Steps { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<Goal> Goals { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<PaymentCategory> PaymentCategories { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PaymentCategory>(entity => {
            entity.Property(c => c.Name).UseCollation("NOCASE");
            entity.Property(c => c.Label).UseCollation("NOCASE");
            entity.HasIndex(c => c.Name).IsUnique();
            entity.HasIndex(c => c.Label).IsUnique();
        });

        modelBuilder.Entity<Payment>(entity => {
            entity.Property(p => p.Currency).HasConversion<string>().HasMaxLength(3);
            entity.HasIndex(p => p.SpentOn);
            entity.HasOne(p => p.Category)
                .WithMany(c => c.Payments)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // SQLite does not keep DateTimeKind; post timestamps are always UTC
        var utcConverter = new ValueConverter<DateTime, DateTime>(
            v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
        );

        modelBuilder.Entity<Post>(entity => {
            entity.Property(p => p.CreatedOn).HasConversion(utcConverter);
            entity.HasIndex(p => p.CreatedOn);
        });

        // Add a ValueConverter for the Color property
        var colorConverter = new ValueConverter<Color, string>(
            v => ColorTranslator.ToHtml(v),
            v => ColorTranslator.FromHtml(v)
        );

        foreach (var entityType in modelBuilder.Model.GetEntityTypes()) {
            var colorProperty = entityType.ClrType
                .GetProperties()
                .FirstOrDefault(p => p.PropertyType == typeof(Color));

            if (colorProperty != null) {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(colorProperty.Name)
                    .HasConversion(colorConverter);
            }
        }
    }
}