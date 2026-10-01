using DevOpsDemo.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=localhost;Port=5432;Database=devopsdemo;Username=devopsuser;Password=devopspassword";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("ui", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("ui");
app.MapControllers();

app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    application = "DevOpsDemo.Api",
    version = Environment.GetEnvironmentVariable("APP_VERSION") ?? "local",
    timestamp = DateTimeOffset.UtcNow
}));

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync();
    await DbSeeder.SeedAsync(db);
}

app.Run();

public partial class Program { }
