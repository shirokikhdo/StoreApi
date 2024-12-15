using Api.Models;
using Bogus;

namespace Api.Seed;

public static class FakeProductGenerator
{
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
            .RuleFor(x => x.Image, y => "https://placehold.co/200")
            .Generate(count);
    }
}