using Aspire.Hosting.Dapr;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var rabbitMq = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin();


builder.AddProject<Projects.Gateway_API>("gateway-api")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "gateway",
        DaprHttpPort = 3500
    });

builder.AddProject<Projects.GaitSession_API>("gaitsession-api")
    .WithReference(rabbitMq)
    .WithDaprSidecar(new DaprSidecarOptions 
    { 
        AppId = "gaitsessionservice",
        DaprHttpPort = 3501
    });

builder.AddProject<Projects.GaitPointData_API>("gaitpointdata-api")
    .WithReference(rabbitMq)
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "gaitpointdataservice",
        DaprHttpPort = 3502
    });

builder.AddProject<Projects.GaitDataOrchestrator_API>("gaitdataorchestrator-api")
    .WithReference(rabbitMq)
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "gaitdataorchestratorservice",
        DaprHttpPort = 3503
    });

#pragma warning disable ASPIREHOSTINGPYTHON001
var pythonApp = builder.AddPythonApp(
    name: "pythonc3dreader",
    projectDirectory: "../Python/PythonC3DReader",
    scriptPath: "main.py",
    virtualEnvironmentPath: "../PythonC3DReader/.venv"
)
#pragma warning restore ASPIREHOSTINGPYTHON001
.WithHttpEndpoint(env: "PORT") // Bruger miljøvariabel til port
.WithDaprSidecar(new DaprSidecarOptions
{
    AppId = "c3dreader",
    DaprHttpPort = 3504
});

// Sæt debug-mode, hvis app’en kører i udvikling
if (builder.ExecutionContext.IsRunMode && builder.Environment.IsDevelopment())
{
    pythonApp.WithEnvironment("DEBUG", "True");
}

builder.Build().Run();