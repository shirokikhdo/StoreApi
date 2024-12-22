using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Api.Extensions;

/// <summary>
/// Расширение для настройки Swagger с пользовательскими параметрами конфигурации.
/// </summary>
public static class SwaggerGenCustomConfigServiceExtension
{
    /// <summary>
    /// Добавляет пользовательскую конфигурацию Swagger в коллекцию служб.
    /// </summary>
    /// <param name="services">Коллекция служб, в которую будет добавлена конфигурация Swagger.</param>
    /// <returns>Обновленная коллекция служб с добавленной конфигурацией Swagger.</returns>
    public static IServiceCollection AddSwaggerGenCustomConfig(
        this IServiceCollection services
    )
    {
        var description = new StringBuilder()
            .Append("Заголовок авторизации JWT с использованием схемы Bearer.")
            .Append(Environment.NewLine)
            .Append(Environment.NewLine)
            .Append("Введите 'bearer' [пробел], а затем свой токен")
            .Append(Environment.NewLine)
            .Append(Environment.NewLine)
            .Append("Пример: \"bearer eyJh.bGc.iOi\"")
            .ToString();

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(
                JwtBearerDefaults.AuthenticationScheme,
                new OpenApiSecurityScheme
                {
                    Description = description,
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
        });

        return services;
    }
}