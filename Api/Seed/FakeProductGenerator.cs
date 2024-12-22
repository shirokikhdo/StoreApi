using Api.Models;
using Bogus;

namespace Api.Seed;

/// <summary>
/// Статический класс для генерации фейковых продуктов.
/// </summary>
public static class FakeProductGenerator
{
    /// <summary>
    /// Генерирует список фейковых продуктов.
    /// </summary>
    /// <param name="count">Количество продуктов для генерации. По умолчанию равно 20.</param>
    /// <returns>Список сгенерированных продуктов.</returns>
    public static List<Product> GenerateProductList(int count = 20)
    {
        var categories = new[] {"Категория 1", "Категория 2", "Категория 3"};
        var specialTags = new[] {"Новинка", "Популярный", "Рекомендуемый"};

        return new Faker<Product>("ru")
            .RuleFor(x => x.Id, y => y.IndexFaker + 1)
            .RuleFor(x => x.Name, y => y.Commerce.ProductName())
            .RuleFor(x => x.Description, y => y.Lorem.Sentence())
            .RuleFor(x => x.Category, y => y.PickRandom(categories))
            .RuleFor(x => x.SpecialTag, y => y.PickRandom(specialTags))
            .RuleFor(x => x.Price, y => Math.Round(y.Random.Double(1, 1000), 2))
            .RuleFor(x => x.Image, y => $"https://s3.timeweb.cloud/30b1c92c-a363-432d-8431-89db9d45df21/img{y.IndexFaker}.png")
            .Generate(count);
    }
}