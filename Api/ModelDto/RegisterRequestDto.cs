﻿namespace Api.ModelDto;

/// <summary>
/// DTO (Data Transfer Object) для запроса регистрации нового пользователя.
/// </summary>
public class RegisterRequestDto
{
    /// <summary>
    /// Получает или задает имя пользователя.
    /// Это обязательное поле, которое должно быть уникальным и использоваться для входа в систему.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Получает или задает адрес электронной почты пользователя.
    /// Это обязательное поле, которое должно быть уникальным и использоваться для подтверждения регистрации.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Получает или задает пароль пользователя.
    /// Это обязательное поле, которое должно соответствовать требованиям безопасности (например, минимальная длина, наличие специальных символов).
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Получает или задает роль пользователя.
    /// Это необязательное поле, которое может использоваться для назначения определенных прав доступа пользователю при регистрации.
    /// </summary>
    public string Role { get; set; }
}