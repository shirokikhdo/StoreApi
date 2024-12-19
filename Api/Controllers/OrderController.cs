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

    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseServer>> GetOrder(int id)
    {
        if (id <= 0)
            return BadRequest(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = { "Неверный идентификатор заказа" }
            });

        try
        {
            var orderHeader = await _ordersService.GetOrderByIdAsync(id);

            if(orderHeader is null)
                return NotFound(new ResponseServer
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = { "Заказ не найден" }
                });

            return Ok(new ResponseServer
            {
                StatusCode = HttpStatusCode.Created,
                Result = orderHeader
            });
        }
        catch (Exception e)
        {
            return BadRequest(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = { "Что-то пошло не так", e.Message }
            });
        }
    }

    [HttpGet]
    public async Task<ActionResult<ResponseServer>> GetOrdersByUserId(string userId)
    {
        try
        {
            var orderHeaders = await _ordersService.GetOrdersByUserIdAsync(userId);

            return Ok(new ResponseServer
            {
                StatusCode = HttpStatusCode.OK,
                Result = orderHeaders
            });
        }
        catch (Exception e)
        {
            return BadRequest(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = { "Что-то пошло не так", e.Message }
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ResponseServer>> UpdateOrderHeader(
        int id, [FromBody] OrderHeaderUpdateDto orderHeaderUpdateDto)
    {
        try
        {
            var success = await _ordersService.UpdateOrderHeaderAsync(id, orderHeaderUpdateDto);
            if (success)
                return Ok(new ResponseServer
                {
                    StatusCode = HttpStatusCode.OK,
                    Result = "Всё обновлено"
                });

            return BadRequest(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = { "Обновление пошло не по плану" }
            });
        }
        catch (Exception e)
        {
            return BadRequest(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = { "Что-то пошло не так", e.Message }
            });
        }
    }
}