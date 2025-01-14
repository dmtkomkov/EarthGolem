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