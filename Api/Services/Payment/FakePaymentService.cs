using System.Net;
using Api.Common;
using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Payment;

public class FakePaymentService : IPaymentService
{
    private const string CARD_NUMBER = "1111 2222 3333 4444";
    private readonly AppDbContext _dbContext;

    public FakePaymentService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ActionResult<ResponseServer>> HandlePaymentAsync(string userId, int orderHeaderId, string cardNumber)
    {
        var shoppingCart = await _dbContext.ShoppingCarts
            .Include(x => x.CartItems)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (shoppingCart is null 
            || shoppingCart.CartItems is null 
            || !shoppingCart.CartItems.Any())
            return new BadRequestObjectResult(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = {"Корзина пуста или не найдена"}
            });

        shoppingCart.TotalAmount = shoppingCart.CartItems
            .Sum(x => x.Product.Price * x.Quantity);

        var order = await _dbContext.OrderHeaders
            .FirstOrDefaultAsync(x => x.OrderHeaderId == orderHeaderId);

        if (order is null)
            return new BadRequestObjectResult(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = { "Такого заказа не существует" }
            });

        await Task.Delay(5000);

        var paymentResponse = cardNumber == CARD_NUMBER
            ? new PaymentResponse
            {
                Success = true,
                IntentId = "fake_intent_id",
                Secret = "fake_secret"
            }
            : new PaymentResponse 
            {
                Success = false,
                IntentId = string.Empty,
                Secret = string.Empty,
                ErrorMessage = "Недействительная карта"
            };

        if (!paymentResponse.Success)
            return new BadRequestObjectResult(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = { paymentResponse.ErrorMessage }
            });

       
        order.Status = SharedData.OrderStatus.ReadyToSheep;
        await _dbContext.SaveChangesAsync();
        
        return new OkObjectResult(new ResponseServer
        {
            StatusCode = HttpStatusCode.OK,
            Result = order
        });
    }
}