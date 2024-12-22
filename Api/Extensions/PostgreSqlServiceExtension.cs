using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions;

/// <summary>
/// Расширение для добавления контекста базы данных PostgreSQL и контекста идентификации в контейнер служб.
/// </summary>
public static class PostgreSqlServiceExtension
{
    /// <summary>
    /// Добавляет контекст базы данных PostgreSQL в коллекцию служб.
    /// </summary>
    /// <param name="services">Коллекция служб, в которую будет добавлен контекст базы данных.</param>
    /// <param name="configuration">Объект конфигурации, содержащий строки подключения и другие настройки.</param>
    public static void AddPostgreSqlDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("PostgreSQLConnection")));
    }

    /// <summary>
    /// Добавляет контекст идентификации для работы с пользователями в базе данных PostgreSQL.
    /// </summary>
    /// <param name="services">Коллекция служб, в которую будет добавлен контекст идентификации.</param>
    public static void AddPostgreSqlIdentityContext(
        this IServiceCollection services)
    {
        services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();
    }
}