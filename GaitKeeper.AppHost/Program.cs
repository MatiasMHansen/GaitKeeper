using Aspire.Hosting;
using Aspire.Hosting.Dapr;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.GaitKeeper_WebAssembly>("gaitkeeper-web");


builder.AddProject<Projects.Gateway_API>("gateway-api")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "gateway",
        DaprHttpPort = 3500
    });

builder.AddProject<Projects.GaitSession_API>("gaitsession-api")
    .WithDaprSidecar(new DaprSidecarOptions 
    { 
        AppId = "gaitsessionservice",
        DaprHttpPort = 3501
    });

builder.AddProject<Projects.GaitPointData_API>("gaitpointdata-api")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "gaitpointdataservice",
        DaprHttpPort = 3502
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
    DaprHttpPort = 3503
});

// Sæt debug-mode, hvis app’en kører i udvikling
if (builder.ExecutionContext.IsRunMode && builder.Environment.IsDevelopment())
{
    pythonApp.WithEnvironment("DEBUG", "True");
}

builder.Build().Run();