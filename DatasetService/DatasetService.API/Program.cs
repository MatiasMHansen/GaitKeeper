using DatasetService.Application;
using DatasetService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddSqlServerDbContext<DatasetContext>("GaitDatasetDB"); // Skal muligvis v�re f�r "AddServiceDefaults"

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddControllers().AddDapr();

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapControllers();

// Configure the HTTP request pipeline.
// app.UseHttpsRedirection(); Dapr k�rer kun med http - ASP.NET redirecter sandsynligvis fra http -> https

app.UseAuthorization();

app.Run();
