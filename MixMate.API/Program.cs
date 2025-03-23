using MixMate.API.GraphQL;
using MixMate.Core.Interfaces;
using MixMate.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Dependency injection
builder.Services.RegisterDatabase();
builder.Services.RegisterServices();
builder.Services.RegisterRepositories();

// Configure GQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

var app = builder.Build();

// Ensure database and tables exist
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
    await context.Initialize();
}

app.MapGraphQL();

app.Run();
