using MixMate.API.GraphQL;
using MixMate.Core.Interfaces;
using MixMate.API;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDataSource("mixmate");

//Dependency injection
builder.Services.RegisterDatabase();
builder.Services.RegisterServices();
builder.Services.RegisterRepositories();

// Configure GQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .ModifyRequestOptions(o =>
    o.IncludeExceptionDetails = builder.Environment.IsDevelopment());

var app = builder.Build();

// Ensure database and tables exist
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
    await context.Initialize();
}

app.MapGraphQL();

app.Run();
