using MixMate.Core.Interfaces;
using MixMate.Core.Services;
using MixMate.DataAccess.Database;
using MixMate.DataAccess.Repositories;

namespace MixMate.Web;

public static class ServiceConfiguration
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IFileProcessingService, FileProcessingService>();
    }

    public static void RegisterDatabase(this IServiceCollection services)
    {
        services.AddSingleton<IDatabaseContext, DatabaseContext>();
    }

    public static void RegisterRepositories(this IServiceCollection services)
    {
       services.AddScoped<ISongRepository, SongRepository>();
    }
}
