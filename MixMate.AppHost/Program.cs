var builder = DistributedApplication.CreateBuilder(args);

//Back-end
var postgres = builder.AddPostgres("postgres").WithLifetime(ContainerLifetime.Persistent).AddDatabase("mixmate");

var api = builder.AddContainer("api", "mixmate-api")
             .WithHttpEndpoint(port: 5165, targetPort: 5165, name: "http")
             .WithDockerfile("../", "MixMate.API/Dockerfile")
             .WithReference(postgres)
             .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
             .WaitFor(postgres);

var apiEndpoint = api.GetEndpoint("http"); //web needs to reference the endpoint, not the container

//Front-end
builder.AddProject<Projects.MixMate_Web>("web")
    .WithExternalHttpEndpoints()
    .WithReference(apiEndpoint)
    .WaitFor(api);

builder.Build().Run();