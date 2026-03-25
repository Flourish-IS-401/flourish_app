using Microsoft.EntityFrameworkCore;
using flourishbackend.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseLower;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var sqliteRelative = builder.Configuration.GetConnectionString("FlourishConnection")
    ?? "Data Source=BeatsMe.sqlite";
// Resolve relative "Data Source=..." against the project directory so `dotnet run` cwd doesn't point at another DB.
var sqliteFileName = sqliteRelative.Replace("Data Source=", "", StringComparison.OrdinalIgnoreCase).Trim();
var sqlitePath = Path.IsPathRooted(sqliteFileName)
    ? sqliteFileName
    : Path.Combine(builder.Environment.ContentRootPath, sqliteFileName);
builder.Services.AddDbContext<FlourishDbContext>(options =>
{
    options.UseSqlite($"Data Source={sqlitePath}");
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();
app.UseCors("AllowFrontend");

// Auto-create the database and tables on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FlourishDbContext>();
    db.Database.EnsureCreated();
    DevUserSeed.EnsureDevUser(db);
    DevUserSeed.EnsureDevBabyProfile(db);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(x => x.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

