using Dapper;
using MixMate.Core.Interfaces;
using MixMate.Core.Services;
using MixMate.DataAccess.Database;
using MixMate.DataAccess.Repositories;
using MixMate.DataAccess.SqlHandlers;

namespace MixMate.Web;

public static class ServiceConfiguration
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<ISongService, SongService>();
        services.AddSingleton<IFileProcessingService, FileProcessingService>();
        services.AddSingleton<IMixingTechnique, SmoothMixingTechnique>();
        services.AddSingleton<IMixingTechnique, EnergyBoostMixingTechnique>();
        services.AddSingleton<IMixingService, MixingService>();
    }

    public static void RegisterDatabase(this IServiceCollection services)
    {
        services.AddSingleton<IDatabaseContext, DatabaseContext>();
        SqlMapper.AddTypeHandler(new TimeSpanHandler());
        SqlMapper.AddTypeHandler(new KeyHandler());
    }

    public static void RegisterRepositories(this IServiceCollection services)
    {
       services.AddSingleton<ISongRepository, SongRepository>();
    }
}
