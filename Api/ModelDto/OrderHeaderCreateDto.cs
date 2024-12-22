using System.ComponentModel.DataAnnotations;

namespace Api.ModelDto;

/// <summary>
/// DTO (Data Transfer Object) для создания заголовка заказа.
/// </summary>
public class OrderHeaderCreateDto
{
    /// <summary>
    /// Получает или задает имя клиента, 
    /// который делает заказ.
    /// </summary>
    [Required]
    public string CustomerName { get; set; }

    /// <summary>
    /// Получает или задает электронную почту клиента, 
    /// которая будет использоваться для связи.
    /// </summary>
    [Required]
    public string CustomerEmail { get; set; }

    /// <summary>
    /// Получает или задает идентификатор пользователя приложения, 
    /// связанного с заказом (если применимо).
    /// </summary>
    public string AppUserId { get; set; }

    /// <summary>
    /// Получает или задает общую сумму заказа.
    /// </summary>
    public double OrderTotalAmount { get; set; }

    /// <summary>
    /// Получает или задает статус заказа, 
    /// например, "Новый", "В обработке", "Завершен" и т.д.
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Получает или задает общее количество товаров в заказе.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Получает или задает детали заказа, 
    /// содержащие информацию о каждом товаре в заказе.
    /// </summary>
    public IEnumerable<OrderDetailsCreateDto> OrderDetailsDto { get; set; }
}