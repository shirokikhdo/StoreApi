namespace Api.Services.Storage;

/// <summary>
/// Представляет настройки для веб-сервиса TimeWeb.
/// </summary>
public class TimeWebSettings
{
    /// <summary>
    /// Получает или задает URL сервиса.
    /// </summary>
    public string ServiceURL { get; set; }

    /// <summary>
    /// Получает или задает регион, в котором размещен сервис.
    /// </summary>
    public string Region { get; set; }

    /// <summary>
    /// Получает или задает ключ доступа для аутентификации в сервисе.
    /// </summary>
    public string AccessKey { get; set; }

    /// <summary>
    /// Получает или задает секретный ключ для аутентификации в сервисе.
    /// </summary>
    public string SecretKey { get; set; }

    /// <summary>
    /// Получает или задает имя бакета, используемого для хранения данных.
    /// </summary>
    public string BucketName { get; set; }
}