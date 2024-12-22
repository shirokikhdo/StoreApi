using Api.Services.Payment;

namespace Api.Extensions;

/// <summary>
/// Расширение для добавления службы обработки платежей в контейнер служб.
/// </summary>
public static class PaymentServiceExtension
{
    /// <summary>
    /// Добавляет службу обработки платежей в коллекцию служб.
    /// </summary>
    /// <param name="services">Коллекция служб, в которую будет добавлена служба обработки платежей.</param>
    /// <returns>Обновленная коллекция служб с добавленной службой обработки платежей.</returns>
    public static IServiceCollection AddPaymentService(this IServiceCollection services) =>
        services.AddScoped<IPaymentService, FakePaymentService>();
}