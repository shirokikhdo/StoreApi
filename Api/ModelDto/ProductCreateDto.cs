using System.ComponentModel.DataAnnotations;

namespace Api.ModelDto;

/// <summary>
/// DTO (Data Transfer Object) для создания нового продукта.
/// </summary>
public class ProductCreateDto
{
    /// <summary>
    /// Получает или задает название продукта.
    /// Это обязательное поле для создания нового продукта.
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Получает или задает описание продукта.
    /// Это обязательное поле, которое должно содержать информацию о продукте.
    /// </summary>
    [Required]
    public string Description { get; set; }

    /// <summary>
    /// Получает или задает специальный тег для продукта.
    /// Это необязательное поле, которое может использоваться для выделения продукта.
    /// </summary>
    public string SpecialTag { get; set; }

    /// <summary>
    /// Получает или задает категорию, к которой принадлежит продукт.
    /// Это необязательное поле, которое может помочь в организации продуктов.
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// Получает или задает цену продукта.
    /// Это обязательное поле, значение должно быть в диапазоне от 1 до 1000.
    /// </summary>
    [Range(1, 1000)]
    public double Price { get; set; }

    /// <summary>
    /// Получает или задает изображение продукта.
    /// Это необязательное поле, которое позволяет загружать файл изображения для продукта.
    /// </summary>
    public IFormFile Image { get; set; }
}