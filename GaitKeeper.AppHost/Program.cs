using Microsoft.Extensions.Hosting;
using CommunityToolkit.Aspire.Hosting.Dapr;

var builder = DistributedApplication.CreateBuilder(args);

var rabbitMq = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin();

var redis = builder.AddRedis("redis", port: 6379);

var minio = builder.AddContainer("minio", "minio/minio", "latest")
    .WithEnvironment("MINIO_ACCESS_KEY", "admin")
    .WithEnvironment("MINIO_SECRET_KEY", "adminadmin")
    .WithArgs("server", "/data", "--console-address", ":9001")
    .WithVolume("minio-data", "/data")
    .WithHttpEndpoint(name: "web", port: 9000, targetPort: 9000)       // MinIO API
    .WithHttpEndpoint(name: "console", port: 9001, targetPort: 9001);   // MinIO UI

builder.AddProject<Projects.Gateway_API>("gateway-api")
    .WaitFor(minio)
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "gateway",
        DaprHttpPort = 3500
    });

builder.AddProject<Projects.GaitSession_API>("gaitsession-api")
    .WithReference(rabbitMq)
    .WaitFor(minio)
    .WithDaprSidecar(new DaprSidecarOptions 
    { 
        AppId = "gaitsessionservice",
        DaprHttpPort = 3501
    });

builder.AddProject<Projects.GaitPointData_API>("gaitpointdata-api")
    .WithReference(rabbitMq)
    .WaitFor(minio)
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "gaitpointdataservice",
        DaprHttpPort = 3502
    });

builder.AddProject<Projects.GaitDataOrchestrator_API>("gaitdataorchestrator-api")
    .WithReference(rabbitMq)
    .WithReference(redis)
    .WaitFor(minio)
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "gaitdataorchestratorservice",
        DaprHttpPort = 3503
    });

#pragma warning disable ASPIREHOSTINGPYTHON001
var pythonApp = builder.AddPythonApp(
    name: "pythonc3dreader",
    appDirectory: "../Python/PythonC3DReader",
    scriptPath: "main.py",
    virtualEnvironmentPath: "../PythonC3DReader/.venv"
)
#pragma warning restore ASPIREHOSTINGPYTHON001
.WithHttpEndpoint(env: "PORT") // Matcher porten i main.py
.WaitFor(minio)
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