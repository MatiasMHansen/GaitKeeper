using Microsoft.Extensions.Hosting;
using CommunityToolkit.Aspire.Hosting.Dapr;

var builder = DistributedApplication.CreateBuilder(args);



var minio = builder.AddContainer("minio", "minio/minio", "latest")
    .WithEnvironment("MINIO_ACCESS_KEY", "admin")
    .WithEnvironment("MINIO_SECRET_KEY", "adminadmin")
    .WithArgs("server", "/data", "--console-address", ":9001")
    .WithVolume("minio-data", "/data")
    .WithHttpEndpoint(name: "web", port: 9000, targetPort: 9000)       // MinIO API
    .WithHttpEndpoint(name: "console", port: 9001, targetPort: 9001);   // MinIO UI

var sqlPassword = builder.AddParameter("sql-password", secret: true);

var sqlServer = builder.AddSqlServer("GaitkeeperSql", password: sqlPassword)
    .WithDataVolume();

var sessionDb = sqlServer.AddDatabase("GaitSessionDB");
var datasetDb = sqlServer.AddDatabase("GaitDatasetDB");

builder.AddProject<Projects.Gateway_API>("gateway-api")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "gateway",
        DaprHttpPort = 3500
    })
    .WaitFor(minio);

builder.AddProject<Projects.GaitSessionService_API>("gaitsession-api")
    .WithReference(sessionDb)
    .WithDaprSidecar(new DaprSidecarOptions 
    { 
        AppId = "gaitsessionservice",
        DaprHttpPort = 3501
    })
    .WaitFor(sessionDb)
    .WaitFor(minio);

builder.AddProject<Projects.GaitPointData_API>("gaitpointdata-api")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "gaitpointdataservice",
        DaprHttpPort = 3502
    })
    .WaitFor(minio);

builder.AddProject<Projects.GaitDataOrchestrator_API>("gaitdataorchestrator-api")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "gaitdataorchestratorservice",
        DaprHttpPort = 3503
    })
    .WaitFor(minio);

builder.AddProject<Projects.DatasetService_API>("datasetservice-api")
    .WithReference(datasetDb)
    .WithDaprSidecar(new DaprSidecarOptions
    {
        AppId = "datasetservice",
        DaprHttpPort = 3504
    })
    .WaitFor(datasetDb)
    .WaitFor(minio);

#pragma warning disable ASPIREHOSTINGPYTHON001
var pythonApp = builder.AddPythonApp(
    name: "pythonc3dreader",
    appDirectory: "../Python/PythonC3DReader",
    scriptPath: "main.py",
    virtualEnvironmentPath: "../PythonC3DReader/.venv"
)
#pragma warning restore ASPIREHOSTINGPYTHON001
.WithHttpEndpoint(env: "PORT") // Matcher porten i main.py
.WithDaprSidecar(new DaprSidecarOptions
{
    AppId = "c3dreader",
    DaprHttpPort = 3505
})
.WaitFor(minio);

// Frontend - Blazor WebAssembly
//builder.AddProject<Projects.GaitKeeper_WebAssembly>("gaitkeeper-client");

// Sæt debug-mode, hvis app’en kører i udvikling
if (builder.ExecutionContext.IsRunMode && builder.Environment.IsDevelopment())
{
    pythonApp.WithEnvironment("DEBUG", "True");
}

builder.Build().Run();