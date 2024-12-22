using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Api.Extensions;

/// <summary>
/// Расширение для настройки аутентификации в приложении.
/// </summary>
public static class AuthentificationServiceExtension
{
    /// <summary>
    /// Добавляет конфигурацию аутентификации в контейнер служб.
    /// </summary>
    /// <param name="services">Коллекция служб, в которую будет добавлена конфигурация аутентификации.</param>
    /// <param name="configuration">Объект конфигурации, используемый для получения настроек аутентификации.</param>
    /// <returns>Обновленная коллекция служб с добавленной конфигурацией аутентификации.</returns>
    public static IServiceCollection AddAuthentificationConfig(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var authSettingsToken = configuration["AuthSettings:SecretKey"];
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(authSettingsToken)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        return services;
    }
}