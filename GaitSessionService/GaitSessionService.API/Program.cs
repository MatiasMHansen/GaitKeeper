using GaitSessionService.Application;
using GaitSessionService.Infrastructure;
// Kun til test under udvikling
using GaitSessionService.Application.Command.CommandDTOs;
using GaitSessionService.Application.Command;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

//builder.AddSqlServerDbContext<GaitSessionContext>("GaitkeeperDB"); 
//, options => options.MigrationsAssembly("GaitSessionService.DatabaseMigration"));
//(builder.Configuration.GetConnectionString("GaitSessionService"));

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
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
app.MapGet("/", () => "Hello from GaitSessionService");

// --------------- EndPoints til tests under udvikling ----------------

app.MapPost("/gaitsession/post", async([FromBody] CreateGaitSessionDTO dto, [FromServices] IGaitSessionCommand command) 
        => await command.CreateAsync(dto));

app.MapDelete("/gaitsession/delete/{id}", async (Guid pointDataId, [FromServices] IGaitSessionCommand command)
        => await command.DeleteAsync(pointDataId));

app.Run();