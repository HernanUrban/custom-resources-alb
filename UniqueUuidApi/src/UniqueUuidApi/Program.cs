using Steeltoe.Discovery.Client;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Management.Endpoint;
using UniqueUuidApi.Configuration;
using UniqueUuidApi.Services;

var builder = WebApplication.CreateBuilder(args);
// Use "docker" profile if environment variable is set
var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";
builder.Configuration.AddJsonFile($"appsettings.{environment}.json", optional: true);
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSingleton<IUuidService, UuidService>();  // Register as Singleton
builder.Services.AddSingleton<IMemoryStatsService, MemoryStatsService>();

// Add Steeltoe Actuator
builder.Services.AddAllActuators();

// Add Eureka
builder.Services.AddEureka();

// Add Swagger for API documentation (optional)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Build the app
var app = builder.Build();

// Access the configuration value
var hostName = app.Configuration["Eureka:Instance:Hostname"];
var ipAddress = app.Configuration["Eureka:Instance:IpAddress"];
Console.WriteLine($"HostName: {hostName}");
Console.WriteLine($"IpAddress: {ipAddress}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapAllActuators();

app.UseAuthorization();
app.MapControllers();
app.Run();
