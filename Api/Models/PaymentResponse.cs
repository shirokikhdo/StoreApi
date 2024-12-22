namespace Api.Models;

/// <summary>
/// Представляет ответ на платежный запрос, содержащий информацию о результате операции.
/// </summary>
public class PaymentResponse
{
    /// <summary>
    /// Получает или задает значение, указывающее, был ли платеж успешным.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Получает или задает идентификатор намерения платежа.
    /// </summary>
    public string IntentId { get; set; }

    /// <summary>
    /// Получает или задает секретный ключ, используемый для подтверждения платежа.
    /// </summary>
    public string Secret { get; set; }

    /// <summary>
    /// Получает или задает сообщение об ошибке, если платеж не был успешным.
    /// </summary>
    public string ErrorMessage { get; set; }
}