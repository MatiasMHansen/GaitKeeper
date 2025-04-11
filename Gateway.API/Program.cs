var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDaprClient(); // Registrerer Dapr Client for service-to-service kald
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy")); // YARP konfiguration

// TILF�J CORS KONFIGURATION
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
        {
            policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost") // ONLY UNDER DEVELOPMENT!!!
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

//// AKTIVER CORS I PIPELINE
app.UseCors("AllowBlazorClient");

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization(); // Ikke implementeret endnu

app.MapDefaultEndpoints(); // Aspire default endpoints
app.MapReverseProxy(); // YARP h�ndterer nu alle ruter baseret p� appsettings.json

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// --------------- EndPoint 'Health check' ----------------
app.MapGet("/", () => "Hello from Gateway");

app.Run();
