var builder = DistributedApplication.CreateBuilder(args);

var server = builder.AddProject<Projects.TaskFlow_Server>("server")
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints();

var web = builder.AddViteApp("web", "../Services/TaskFlow.Web")
    .WithRunScript("start")
    .WithReference(server)
    .WaitFor(server);

server.PublishWithContainerFiles(web, "wwwroot");

builder.Build().Run();
