
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// --------------- EndPoints ----------------
app.MapGet("/", () => "Hello from SubjectService");

//app.MapGet("/PreviewMetadata/{file_name}", async (string file_name) =>
//{
//    // Opret en DaprClient for service invocation
//    var daprClient = new DaprClientBuilder().Build();

//    // Kald PythonC3DReader via Dapr. 
//    // "pythonc3dreader" er Dapr-app id for PythonC3DReader (som defineret i din Program.cs)
//    // Vi kalder /metadata/{file_name} metoden.
//    var metadata = await daprClient.InvokeMethodAsync<object>(
//        HttpMethod.Get,
//        "pythonc3dreader",
//        $"metadata/{file_name}");

//    return Results.Ok(metadata);
//});


app.Run();
