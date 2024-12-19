using Api.Data;
using Api.Models;
using Api.Services.Payment;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class PaymentController : StoreController
{
    private readonly IPaymentService _paymentService;

    public PaymentController(
        AppDbContext dbContext,
        IPaymentService paymentService) 
        : base(dbContext)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<ActionResult<ResponseServer>> MakePayment(
        string userId, int orderHeaderId, string cardNumber) =>
        await _paymentService.HandlePaymentAsync(userId, orderHeaderId, cardNumber);
}