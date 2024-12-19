using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services.Payment;

public interface IPaymentService
{
    Task<ActionResult<ResponseServer>> HandlePaymentAsync(
        string userId, 
        int orderHeaderId, 
        string cardNumber);
}