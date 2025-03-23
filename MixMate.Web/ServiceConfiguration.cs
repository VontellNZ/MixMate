using MixMate.Core.Interfaces;
using MixMate.Core.Services;

namespace MixMate.Web;

public static class ServiceConfiguration
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IFileProcessingService, FileProcessingService>();
        services.AddSingleton<IMixingTechnique, SmoothMixingTechnique>();
        services.AddSingleton<IMixingTechnique, EnergyBoostMixingTechnique>();
        services.AddSingleton<IMixingService, MixingService>();
    }
}
