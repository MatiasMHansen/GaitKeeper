using Aspire.Hosting.Dapr;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Subject_API>("subject-api")
    .WithDaprSidecar(new DaprSidecarOptions 
    { 
        AppId = "subjectservice",
        DaprHttpPort = 3501
    });

builder.AddProject<Projects.GaitCycle_API>("gaitcycle-api")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "gaitcycleservice",
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