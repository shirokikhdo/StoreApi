using System.Net;
using Api.Data;
using Api.ModelDto;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class OrderController : StoreController
{
    private readonly OrdersService _ordersService;

    public OrderController(
        AppDbContext dbContext,
        OrdersService ordersService) 
        : base(dbContext)
    {
        _ordersService = ordersService;
    }

    [HttpPost]
    public async Task<ActionResult<ResponseServer>> CreateOrder(
        [FromBody] OrderHeaderCreateDto orderHeaderCreateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = {"Неверное состояние модели заказа"}
            });

        try
        {
            var order = await _ordersService.CreateOrderAsync(orderHeaderCreateDto);

            return Ok(new ResponseServer
            {
                StatusCode = HttpStatusCode.Created,
                Result = order
            });
        }
        catch (Exception e)
        {
            return BadRequest(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = { "Невероятная ошибка", e.Message }
            });
        }
    }
}