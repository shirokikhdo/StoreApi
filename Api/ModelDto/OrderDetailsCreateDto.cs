using System.ComponentModel.DataAnnotations;

namespace Api.ModelDto;

/// <summary>
/// DTO (Data Transfer Object) для создания деталей заказа.
/// </summary>
public class OrderDetailsCreateDto
{
    /// <summary>
    /// Получает или задает идентификатор продукта, 
    /// который добавляется в заказ.
    /// </summary>
    [Required]
    public int ProductId { get; set; }

    /// <summary>
    /// Получает или задает количество продукта, 
    /// которое необходимо заказать.
    /// </summary>
    [Required]
    public int Quantity { get; set; }

    /// <summary>
    /// Получает или задает название продукта, 
    /// который добавляется в заказ.
    /// </summary>
    [Required]
    public string ItemName { get; set; }

    /// <summary>
    /// Получает или задает цену за единицу продукта, 
    /// который добавляется в заказ.
    /// </summary>
    [Required]
    public double Price { get; set; }
}