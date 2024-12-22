using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

/// <summary>
/// Представляет корзину покупок пользователя.
/// </summary>
public class ShoppingCart
{
    /// <summary>
    /// Получает или задает уникальный идентификатор корзины.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Получает или задает идентификатор пользователя, которому принадлежит корзина.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Получает или задает коллекцию элементов корзины.
    /// </summary>
    public ICollection<CartItem> CartItems { get; set; }

    /// <summary>
    /// Получает или задает общую сумму стоимости всех элементов в корзине.
    /// Это свойство не отображается в базе данных.
    /// </summary>
    [NotMapped]
    public double TotalAmount { get; set; }
}