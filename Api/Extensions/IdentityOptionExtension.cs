using Microsoft.AspNetCore.Identity;

namespace Api.Extensions;

/// <summary>
/// Расширение для настройки параметров идентификации в приложении.
/// </summary>
public static class IdentityOptionExtension
{
    /// <summary>
    /// Добавляет конфигурацию параметров идентификации в контейнер служб.
    /// </summary>
    /// <param name="services">Коллекция служб, в которую будет добавлена конфигурация параметров идентификации.</param>
    /// <returns>Обновленная коллекция служб с добавленной конфигурацией параметров идентификации.</returns>
    public static IServiceCollection AddConfigureIdentityOptions(this IServiceCollection services)
    {
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 1;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        });

        return services;
    }
}