using System.ComponentModel.DataAnnotations;

namespace Api.ModelDto;

/// <summary>
/// DTO (Data Transfer Object) для обновления заголовка заказа.
/// </summary>
public class OrderHeaderUpdateDto
{
    /// <summary>
    /// Получает или задает идентификатор заголовка заказа.
    /// Это обязательное поле для идентификации обновляемого заказа.
    /// </summary>
    [Required]
    public int OrderHeaderId { get; set; }

    /// <summary>
    /// Получает или задает имя клиента, 
    /// которое будет обновлено в заголовке заказа.
    /// </summary>
    public string CustomerName { get; set; }

    /// <summary>
    /// Получает или задает электронную почту клиента, 
    /// которую необходимо обновить в заголовке заказа.
    /// </summary>
    public string CustomerEmail { get; set; }

    /// <summary>
    /// Получает или задает статус заказа, 
    /// который будет обновлен, например, "В обработке", "Завершен" и т.д.
    /// </summary>
    public string Status { get; set; }
}