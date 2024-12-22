namespace Api.ModelDto;

/// <summary>
/// DTO (Data Transfer Object) для ответа на запрос входа пользователя.
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// Получает или задает адрес электронной почты пользователя, 
    /// который успешно вошел в систему.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Получает или задает токен доступа, 
    /// выданный пользователю после успешной аутентификации.
    /// </summary>
    public string Token { get; set; }
}