using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TimeTracker;
using TimeTracker.Data;
using TimeTracker.Interfaces;
using TimeTracker.Services;
using TimeTracker.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);

//===================================================================================================================//

const string bearer = "Bearer";

var jwtSettings = builder.Configuration.GetSection("Jwt");
var corsSettings = builder.Configuration.GetSection("CorsSettings");
var tokenValidationParameters = new TokenValidationParameters {
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!)),
    ClockSkew = TimeSpan.Zero
};
var openApiSecurityScheme = new NSwag.OpenApiSecurityScheme
{
    Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
    Name = "Authorization",
    In = NSwag.OpenApiSecurityApiKeyLocation.Header,
    Description = $"Enter token in format: {bearer} <token>"
};

//===================================================================================================================//

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson(options => {
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.Converters.Add(new ColorJsonConverter());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("StoneGolemLocal", policy => {
        policy.WithOrigins(corsSettings.GetSection("AllowedOrigins").Get<string[]>()!)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "TimeTracker API";
    config.AddSecurity(bearer, openApiSecurityScheme);
    config.OperationProcessors.Add(
        new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor(bearer)
    );
});


builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(options => {
        options.DefaultAuthenticateScheme = bearer;
        options.DefaultChallengeScheme = bearer;
    })
    .AddJwtBearer(bearer, options => {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = tokenValidationParameters;
    });

builder.Services.AddScoped<IStepRepository, StepRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IGoalRepository, GoalRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

//===================================================================================================================//

if (args.Length > 0 && args[0] == "create-user")
{
    await UserCreator.CreateUserAsync(app.Services, args);
    return;
}

//===================================================================================================================//

app.UseCors("StoneGolemLocal");

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
}
else if (app.Environment.IsProduction()) {
    app.UseOpenApi(configure =>
        configure.PostProcess = (document, _) => document.Schemes = new[] { NSwag.OpenApiSchema.Https }
    );
}

app.UseSwaggerUi();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();