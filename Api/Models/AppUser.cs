using Microsoft.AspNetCore.Identity;

namespace Api.Models;

/// <summary>
/// Представляет пользователя приложения, наследуемого от <see cref="IdentityUser"/>.
/// </summary>
public class AppUser : IdentityUser
{
    /// <summary>
    /// Получает или задает имя пользователя.
    /// Это поле используется для хранения имени пользователя в приложении.
    /// </summary>
    public string FirstName { get; set; }
}