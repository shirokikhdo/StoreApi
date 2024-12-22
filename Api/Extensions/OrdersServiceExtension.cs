using Api.Services;

namespace Api.Extensions;

/// <summary>
/// Расширение для добавления службы обработки заказов в контейнер служб.
/// </summary>
public static class OrdersServiceExtension
{
    /// <summary>
    /// Добавляет службу обработки заказов в коллекцию служб.
    /// </summary>
    /// <param name="services">Коллекция служб, в которую будет добавлена служба обработки заказов.</param>
    /// <returns>Обновленная коллекция служб с добавленной службой обработки заказов.</returns>
    public static IServiceCollection AddOrdersService(
        this IServiceCollection services) =>
        services.AddScoped<OrdersService>();
}