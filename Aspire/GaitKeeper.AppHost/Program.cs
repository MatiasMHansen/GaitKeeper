using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Subject_API>("subjectservice")
    .WithDaprSidecar();

builder.AddProject<Projects.GaitCycle_API>("gaitcycleservice")
    .WithDaprSidecar();

#pragma warning disable ASPIREHOSTINGPYTHON001
var pythonApp = builder.AddPythonApp(
    name: "pythonc3dreader",
    projectDirectory: "../Python/PythonC3DReader",
    scriptPath: "main.py",
    virtualEnvironmentPath: "../PythonC3DReader/.venv"
)
#pragma warning restore ASPIREHOSTINGPYTHON001
.WithHttpEndpoint(env: "PORT") // Bruger milj�variabel til port
.WithDaprSidecar();

// S�t debug-mode, hvis app�en k�rer i udvikling
if (builder.ExecutionContext.IsRunMode && builder.Environment.IsDevelopment())
{
    pythonApp.WithEnvironment("DEBUG", "True");
}

builder.Build().Run();
