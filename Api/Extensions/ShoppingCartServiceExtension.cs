using Api.Services;

namespace Api.Extensions;

/// <summary>
/// Расширение для добавления службы корзины покупок в контейнер служб.
/// </summary>
public static class ShoppingCartServiceExtension
{
    /// <summary>
    /// Добавляет службу корзины покупок в коллекцию служб.
    /// </summary>
    /// <param name="services">Коллекция служб, в которую будет добавлена служба корзины покупок.</param>
    /// <returns>Обновленная коллекция служб с добавленной службой корзины покупок.</returns>
    public static IServiceCollection AddShoppingCartService(
        this IServiceCollection services) => 
        services.AddScoped<ShoppingCartService>();
}