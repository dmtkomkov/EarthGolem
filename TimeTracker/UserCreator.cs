using Microsoft.AspNetCore.Identity;

namespace TimeTracker;

public static class UserCreator {
    public static async Task CreateUserAsync(IServiceProvider services, string[] args) {
        using var scope = services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var username = "admin";
        var password = "Admin@123";
        var email = "admin@example.com";

        for (int i = 1; i < args.Length; i++) {
            if (args[i] == "--username" && i + 1 < args.Length) {
                username = args[++i];
            }
            else if (args[i] == "--password" && i + 1 < args.Length) {
                password = args[++i];
            }
            else if (args[i] == "--email" && i + 1 < args.Length) {
                email = args[++i];
            }
        }

        var user = await userManager.FindByNameAsync(username);
        if (user != null) {
            Console.WriteLine("User already exists.");
            return;
        }

        var result = await userManager.CreateAsync(new IdentityUser {
            UserName = username,
            Email = email
        }, password);

        if (result.Succeeded) {
            Console.WriteLine("User was created successfully.");
        }
        else {
            foreach (var e in result.Errors) {
                Console.WriteLine($"Error: {e.Description}");
            }
        }
    }
}