using Api.Services.Payment;

namespace Api.Extensions;

public static class PaymentServiceExtension
{
    public static IServiceCollection AddPaymentService(this IServiceCollection services) =>
        services.AddScoped<IPaymentService, FakePaymentService>();
}