using Api.Services.Storage;

namespace Api.Extensions;

/// <summary>
/// Расширение для настройки службы хранения файлов в приложении.
/// </summary>
public static class FileStorageServiceExtension
{
    /// <summary>
    /// Добавляет службу хранения файлов в контейнер служб.
    /// </summary>
    /// <param name="services">Коллекция служб, в которую будет добавлена служба хранения файлов.</param>
    /// <param name="configuration">Объект конфигурации, используемый для получения настроек службы хранения файлов.</param>
    /// <returns>Обновленная коллекция служб с добавленной службой хранения файлов.</returns>
    public static IServiceCollection AddFileStorageService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.Configure<TimeWebSettings>(configuration.GetSection("TimeWebS3"));
        return services;
    }
}