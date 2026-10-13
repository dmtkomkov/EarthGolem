using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TimeTracker.Data;
using TimeTracker.Mappers;
using TimeTracker.Models;

namespace TimeTracker;

public static class PaymentCategorySeeder {
    private const string DefaultFile = "Data/Fixtures/payment-categories.json";

    private record CategoryFixture(int Id, string Name, string? Label, string? Description);

    // Adds the payment categories of a fixture file with their ids. Categories whose id, name or label already
    // exists are skipped, so the command can be run again safely.
    public static async Task SeedAsync(IServiceProvider services, string[] args) {
        var file = DefaultFile;

        for (int i = 1; i < args.Length; i++) {
            if (args[i] == "--file" && i + 1 < args.Length) {
                file = args[++i];
            }
        }

        if (!File.Exists(file)) {
            Fail($"Fixture file not found: {Path.GetFullPath(file)}");
            return;
        }

        var fixtures = JsonConvert.DeserializeObject<List<CategoryFixture>>(await File.ReadAllTextAsync(file)) ?? [];

        var errors = Validate(fixtures);
        if (errors.Count > 0) {
            errors.ForEach(e => Console.WriteLine($"Error: {e}"));
            Fail("Fixture file is invalid; nothing was added.");
            return;
        }

        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if ((await context.Database.GetPendingMigrationsAsync()).Any()) {
            Fail("The database has pending migrations. Run 'dotnet ef database update' first.");
            return;
        }

        var existing = await context.PaymentCategories.AsNoTracking().ToListAsync();
        var added = 0;

        foreach (var fixture in fixtures) {
            var category = new PaymentCategory {
                Id = fixture.Id,
                Name = fixture.Name.Trim(),
                Label = PaymentCategoryMapper.NormalizeLabel(fixture.Label),
                Description = fixture.Description?.Trim() ?? string.Empty,
            };

            var conflict = FindConflict(existing, category);
            if (conflict != null) {
                Console.WriteLine($"Skipped {category.Id} {category.Name}: {conflict}");
                continue;
            }

            context.PaymentCategories.Add(category);
            existing.Add(category);
            added++;
        }

        await context.SaveChangesAsync();
        Console.WriteLine($"Payment categories added: {added}, skipped: {fixtures.Count - added}.");
    }

    private static List<string> Validate(List<CategoryFixture> fixtures) {
        var errors = new List<string>();

        foreach (var fixture in fixtures) {
            var name = fixture.Name?.Trim() ?? string.Empty;
            var label = PaymentCategoryMapper.NormalizeLabel(fixture.Label);

            if (fixture.Id < 1) errors.Add($"Category '{name}': id must be positive.");
            if (name.Length == 0) errors.Add($"Category {fixture.Id}: name is required.");
            if (name.Length > 36) errors.Add($"Category {fixture.Id}: name is longer than 36 characters.");
            if (label?.Length > 4) errors.Add($"Category {fixture.Id}: label is longer than 4 characters.");
            if (fixture.Description?.Trim().Length > 500) {
                errors.Add($"Category {fixture.Id}: description is longer than 500 characters.");
            }
        }

        AddDuplicates(errors, "id", fixtures.Select(f => f.Id.ToString()));
        AddDuplicates(errors, "name", fixtures.Select(f => f.Name?.Trim()));
        AddDuplicates(errors, "label", fixtures.Select(f => PaymentCategoryMapper.NormalizeLabel(f.Label)));

        return errors;
    }

    private static void AddDuplicates(List<string> errors, string field, IEnumerable<string?> values) {
        var duplicates = values
            .Where(v => !string.IsNullOrEmpty(v))
            .GroupBy(v => v!, StringComparer.OrdinalIgnoreCase)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        errors.AddRange(duplicates.Select(d => $"Duplicate {field} '{d}' in the fixture file."));
    }

    // Names and labels are unique case-insensitively, as enforced by the NOCASE column collation
    private static string? FindConflict(List<PaymentCategory> existing, PaymentCategory category) {
        if (existing.Any(c => c.Id == category.Id)) {
            return "id already exists";
        }

        if (existing.Any(c => string.Equals(c.Name, category.Name, StringComparison.OrdinalIgnoreCase))) {
            return "name already exists";
        }

        if (category.Label != null &&
            existing.Any(c => string.Equals(c.Label, category.Label, StringComparison.OrdinalIgnoreCase))) {
            return "label already exists";
        }

        return null;
    }

    private static void Fail(string message) {
        Console.WriteLine($"Error: {message}");
        Environment.ExitCode = 1;
    }
}
