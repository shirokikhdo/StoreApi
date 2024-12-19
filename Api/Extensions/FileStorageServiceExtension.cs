using Api.Services.Storage;

namespace Api.Extensions;

public static class FileStorageServiceExtension
{
    public static IServiceCollection AddFileStorageService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.Configure<TimeWebSettings>(configuration.GetSection("TimeWebS3"));
        return services;
    }
}