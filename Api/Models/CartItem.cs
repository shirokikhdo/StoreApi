using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

/// <summary>
/// Представляет элемент корзины покупок.
/// </summary>
public class CartItem
{
    /// <summary>
    /// Получает или задает уникальный идентификатор элемента корзины.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Получает или задает идентификатор продукта, который представлен в этом элементе корзины.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Получает или задает количество данного продукта в корзине.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Получает или задает идентификатор корзины покупок, к которой принадлежит этот элемент.
    /// </summary>
    public int ShoppingCartId { get; set; }

    /// <summary>
    /// Получает или задает продукт, связанный с этим элементом корзины.
    /// </summary>
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
}