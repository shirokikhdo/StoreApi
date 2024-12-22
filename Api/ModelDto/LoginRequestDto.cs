namespace Api.ModelDto;

/// <summary>
/// DTO (Data Transfer Object) для запроса на вход пользователя.
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// Получает или задает адрес электронной почты пользователя.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Получает или задает пароль пользователя.
    /// </summary>
    public string Password { get; set; }
}