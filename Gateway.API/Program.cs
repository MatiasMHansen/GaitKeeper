var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDaprClient(); // Registrerer Dapr Client for service-to-service kald
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy")); // YARP konfiguration


var app = builder.Build();

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization(); // Ikke implementeret endnu

app.MapDefaultEndpoints(); // Aspire default endpoints
app.MapReverseProxy(); // YARP håndterer nu alle ruter baseret på appsettings.json

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// --------------- EndPoint 'Health check' ----------------
app.MapGet("/", () => "Hello from Gateway");

app.Run();
