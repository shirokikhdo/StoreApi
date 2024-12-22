using Api.Common;
using Microsoft.AspNetCore.Identity;

namespace Api.Extensions;

/// <summary>
/// Расширение для инициализации ролей в системе идентификации.
/// </summary>
public static class RoleInitializerServiceExtension
{
    /// <summary>
    /// Асинхронно инициализирует роли в системе идентификации.
    /// </summary>
    /// <param name="serviceProvider">Объект службы, используемый для разрешения зависимостей.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    public static async Task InitializeRoleAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope
            .ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in SharedData.Roles.AllRoles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}