using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services.Payment;

/// <summary>
/// Интерфейс для сервиса обработки платежей.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Асинхронно обрабатывает платеж для указанного пользователя и заказа.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя, инициирующего платеж.</param>
    /// <param name="orderHeaderId">Идентификатор заголовка заказа.</param>
    /// <param name="cardNumber">Номер карты для обработки платежа.</param>
    /// <returns>Асинхронная задача, представляющая результат выполнения операции в виде объекта <see cref="ActionResult{ResponseServer}"/>.</returns>
    Task<ActionResult<ResponseServer>> HandlePaymentAsync(
        string userId, 
        int orderHeaderId, 
        string cardNumber);
}