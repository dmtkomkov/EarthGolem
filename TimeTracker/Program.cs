using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NSwag.AspNetCore;
using TimeTracker.Data;
using TimeTracker.Interfaces;
using TimeTracker.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers().AddNewtonsoftJson(options => {
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IStepRepository, StepRepository>();

var app = builder.Build();

if (args.Length > 0 && args[0] == "create-user")
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    
    var username = "admin";
    var password = "Admin@123";
    var email = "admin@example.com";
    
    for (int i = 1; i < args.Length; i++)
    {
        if (args[i] == "--username" && i + 1 < args.Length)
        {
            username = args[++i];
        }
        else if (args[i] == "--password" && i + 1 < args.Length)
        {
            password = args[++i];
        }
        else if (args[i] == "--email" && i + 1 < args.Length)
        {
            email = args[++i];
        }
    }

    var user = await userManager.FindByNameAsync(username);
    if (user != null)
    {
        Console.WriteLine("User already exists.");
        return;
    }

    var result = await userManager.CreateAsync(new IdentityUser
    {
        UserName = username,
        Email = email
    }, password);

    if (result.Succeeded)
    {
        Console.WriteLine("User was created successfully.");
    }
    else
    {
        foreach (var e in result.Errors)
        {
            Console.WriteLine($"Error: {e.Description}");
        }
    }
    
    return;
}

//===================================================================================================================//

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/openapi");
    app.UseSwaggerUi(settings =>
    {
        settings.SwaggerRoutes.Add(new SwaggerUiRoute("v1", "/openapi")); 
    });
}

app.MapControllers();

app.MapGroup("/auth").WithTags("Auth").MapIdentityApi<IdentityUser>();

app.Run();