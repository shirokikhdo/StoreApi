using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services.Payment;

public class FakePaymentService : IPaymentService
{
    public async Task<ActionResult<ResponseServer>> HandlePaymentAsync(string userId, string orderHeaderId, string cardNumber)
    {
        throw new NotImplementedException();
    }
}