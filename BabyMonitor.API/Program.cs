using System.Text.Json.Serialization;
using BabyMonitor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------------------
// Services
// ---------------------------------------------------------------------------

// EF Core: PostgreSQL via Npgsql provider. Connection string is read from
// configuration ("ConnectionStrings:LunaDb"); in production it is supplied
// via the env var ConnectionStrings__LunaDb (set in docker-compose).
var connectionString = builder.Configuration.GetConnectionString("LunaDb")
    ?? throw new InvalidOperationException(
        "Connection string 'LunaDb' is not configured. " +
        "Set it in appsettings.Development.json (dev) or via the env var " +
        "ConnectionStrings__LunaDb (prod / docker).");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// MVC Controllers with JSON enum-as-string serialization
builder.Services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()));

// OpenAPI / Swagger for interactive API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Health checks (will be extended later with DB connectivity probe)
builder.Services.AddHealthChecks();

// CORS: permissive only in development so the Angular frontend can call us
// from localhost:4200 (ng serve) and localhost:8080 (nginx container).
const string DevCorsPolicy = "DevClient";
builder.Services.AddCors(options =>
{
    options.AddPolicy(DevCorsPolicy, policy =>
        policy.WithOrigins(
                "http://localhost:4200",
                "http://localhost:8080")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// ---------------------------------------------------------------------------
// Pipeline
// ---------------------------------------------------------------------------

var app = builder.Build();

// Auto-migrate on startup (safe for single-instance self-hosted deployments).
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS is always enabled so the nginx-proxied frontend and direct dev
// requests both work without issues.
app.UseCors(DevCorsPolicy);

app.MapControllers();
app.MapHealthChecks("/health");
app.MapGet("/", () => "Luna API is running");

app.Run();
