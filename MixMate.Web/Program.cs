using MixMate.Core.Interfaces;
using MixMate.Core.Services;
using MixMate.DataAccess.Database;
using MixMate.DataAccess.Repositories;
using MixMate.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register services
builder.Services.AddScoped<IFileProcessingService, FileProcessingService>();

// Register IDatabaseContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("The DefaultConnection connection string is not configured.");

builder.Services.AddSingleton<IDatabaseContext>(provider =>
    new DatabaseContext(connectionString));

// Register repositories
builder.Services.AddScoped<ISongRepository, SongRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();