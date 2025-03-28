using GaitPointData.Application;
using GaitPointData.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddOpenApi();
builder.Services.AddControllers().AddDapr();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseCloudEvents(); // Dapr will send serialized event object vs. being raw CloudEvent
app.MapSubscribeHandler(); // needed for Dapr pub/sub routing

// MapSubscribeHandler & UseCloudEvents skal kaldes før MapControllers?
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection(); Dapr kører kun med http - ASP.NET redirecter sandsynligvis fra http -> https

// --------------- EndPoint 'Health check' ----------------
app.MapGet("/", () => "Hello from GaitPointDataService");

app.Run();