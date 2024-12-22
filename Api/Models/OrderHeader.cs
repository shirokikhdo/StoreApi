using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

/// <summary>
/// Представляет заголовок заказа, содержащий информацию о клиенте и деталях заказа.
/// </summary>
public class OrderHeader
{
    /// <summary>
    /// Получает или задает уникальный идентификатор заголовка заказа.
    /// </summary>
    [Key]
    public int OrderHeaderId { get; set; }
    
    /// <summary>
    /// Получает или задает имя клиента, сделавшего заказ.
    /// </summary>
    [Required]
    public string CustomerName { get; set; }

    /// <summary>
    /// Получает или задает электронную почту клиента.
    /// </summary>
    [Required]
    public string CustomerEmail { get; set; }

    /// <summary>
    /// Получает или задает идентификатор пользователя приложения, связанного с заказом.
    /// </summary>
    public string AppUserId { get; set; }

    /// <summary>
    /// Получает или задает общую сумму заказа.
    /// </summary>
    [ForeignKey("AppUserId")]
    public double TotalOrderAmount { get; set; }

    /// <summary>
    /// Получает или задает дату и время создания заказа.
    /// </summary>
    public DateTime OrderDateTime { get; set; }

    /// <summary>
    /// Получает или задает статус заказа (например, "Обрабатывается", "Доставлен").
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Получает или задает общее количество товаров в заказе.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Получает или задает коллекцию деталей заказа, связанных с этим заголовком.
    /// </summary>
    public IEnumerable<OrderDetails> OrderDetailItems { get; set; }
}