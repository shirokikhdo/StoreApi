using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

/// <summary>
/// Представляет детали заказа, включая информацию о продукте и его количестве.
/// </summary>
public class OrderDetails
{
    /// <summary>
    /// Получает или задает уникальный идентификатор детали заказа.
    /// </summary>
    [Key]
    public int OrderDetailId { get; set; }

    /// <summary>
    /// Получает или задает идентификатор заголовка заказа, к которому принадлежат эти детали.
    /// </summary>
    [Required]
    public int OrderHeaderId { get; set; }

    /// <summary>
    /// Получает или задает идентификатор продукта, который представлен в этих деталях заказа.
    /// </summary>
    [Required]
    public int ProductId { get; set; }

    /// <summary>
    /// Получает или задает продукт, связанный с этими деталями заказа.
    /// </summary>
    [ForeignKey("ProductId")]
    public Product Product { get; set; }

    /// <summary>
    /// Получает или задает количество данного продукта в заказе.
    /// </summary>
    [Required]
    public int Quantity { get; set; }

    /// <summary>
    /// Получает или задает название продукта, представленного в деталях заказа.
    /// </summary>
    [Required]
    public string ItemName { get; set; }

    /// <summary>
    /// Получает или задает цену за единицу продукта.
    /// </summary>
    [Required]
    public double Price { get; set; }
}