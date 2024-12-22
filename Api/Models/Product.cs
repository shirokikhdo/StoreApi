using System.ComponentModel.DataAnnotations;

namespace Api.Models;

/// <summary>
/// Представляет продукт с его основными характеристиками.
/// </summary>
public class Product
{
    /// <summary>
    /// Получает или задает уникальный идентификатор продукта.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Получает или задает название продукта. Это обязательное поле.
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Получает или задает описание продукта. Это обязательное поле.
    /// </summary>
    [Required]
    public string Description { get; set; }

    /// <summary>
    /// Получает или задает специальный тег для продукта, если таковой имеется.
    /// </summary>
    public string SpecialTag { get; set; }

    /// <summary>
    /// Получает или задает категорию, к которой принадлежит продукт.
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// Получает или задает цену продукта. Должна находиться в диапазоне от 1 до 1000.
    /// </summary>
    [Range(1, 1000)]
    public double Price { get; set; }

    /// <summary>
    /// Получает или задает URL изображения продукта.
    /// </summary>
    public string Image { get; set; }
}