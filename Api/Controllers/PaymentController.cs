using Api.Data;
using Api.Models;
using Api.Services.Payment;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Контроллер для управления процессом платежей в магазине.
/// </summary>
public class PaymentController : StoreController
{
    private readonly IPaymentService _paymentService;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="PaymentController"/> с заданным контекстом базы данных и сервисом платежей.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных для доступа к данным.</param>
    /// <param name="paymentService">Сервис для обработки платежей.</param>
    public PaymentController(
        AppDbContext dbContext,
        IPaymentService paymentService) 
        : base(dbContext)
    {
        _paymentService = paymentService;
    }

    /// <summary>
    /// Обрабатывает платеж по указанному идентификатору пользователя и заголовку заказа с использованием номера карты.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя, осуществляющего платеж.</param>
    /// <param name="orderHeaderId">Идентификатор заголовка заказа, для которого осуществляется платеж.</param>
    /// <param name="cardNumber">Номер карты, используемой для платежа.</param>
    /// <returns>Результат операции в виде <see cref="ResponseServer"/>, содержащий информацию о результате платежа.</returns>
    [HttpPost]
    public async Task<ActionResult<ResponseServer>> MakePayment(
        string userId, int orderHeaderId, string cardNumber) =>
        await _paymentService.HandlePaymentAsync(userId, orderHeaderId, cardNumber);
}