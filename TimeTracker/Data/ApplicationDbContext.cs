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

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

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