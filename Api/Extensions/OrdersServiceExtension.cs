using Api.Services;

namespace Api.Extensions;

public static class OrdersServiceExtension
{
    public static IServiceCollection AddOrdersService(
        this IServiceCollection services) =>
        services.AddScoped<OrdersService>();
}