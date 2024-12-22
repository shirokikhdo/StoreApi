using Api.Services;

namespace Api.Extensions;

/// <summary>
/// Расширение для добавления службы генерации JWT токенов в контейнер служб.
/// </summary>
public static class JwtTokenGeneratorServiceExtension
{
    /// <summary>
    /// Добавляет службу генерации JWT токенов в коллекцию служб.
    /// </summary>
    /// <param name="services">Коллекция служб, в которую будет добавлена служба генерации JWT токенов.</param>
    /// <returns>Обновленная коллекция служб с добавленной службой генерации JWT токенов.</returns>
    public static IServiceCollection AddJwtTokenGenerator(this IServiceCollection services) =>
        services.AddScoped<JwtTokenGenerator>();
}