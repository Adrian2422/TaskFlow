var builder = DistributedApplication.CreateBuilder(args);

var sqlPassword = builder.AddParameter("sql-password", secret: true);

var db = builder
    .AddSqlServer("db", password: sqlPassword)
    .WithDataVolume("taskflow-data")
    .WithLifetime(ContainerLifetime.Session)
    .AddDatabase("taskflow-db");

var api = builder.AddProject<Projects.TaskFlow_Api>("api")
    .WithHttpHealthCheck("/health")
    .WithReference(db)
    .WithExternalHttpEndpoints()
    .WaitFor(db);

var web = builder.AddViteApp("web", "../TaskFlow.Web")
    .WithRunScript("start")
    .WithReference(api)
    .WaitFor(api);

api.PublishWithContainerFiles(web, "wwwroot");

builder.Build().Run();
