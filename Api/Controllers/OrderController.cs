using System.Net;
using Api.Data;
using Api.ModelDto;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Контроллер для управления заказами в магазине.
/// </summary>
public class OrderController : StoreController
{
    private readonly OrdersService _ordersService;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="OrderController"/> с заданным контекстом базы данных и сервисом заказов.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных для доступа к данным.</param>
    /// <param name="ordersService">Сервис для работы с заказами.</param>
    public OrderController(
        AppDbContext dbContext,
        OrdersService ordersService) 
        : base(dbContext)
    {
        _ordersService = ordersService;
    }

    /// <summary>
    /// Создает новый заказ на основе предоставленных данных.
    /// </summary>
    /// <param name="orderHeaderCreateDto">Объект, содержащий данные для создания заказа.</param>
    /// <returns>Результат операции в виде <see cref="ResponseServer"/>, содержащий информацию о созданном заказе или ошибке.</returns>
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

    /// <summary>
    /// Получает информацию о заказе по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор заказа.</param>
    /// <returns>Результат операции в виде <see cref="ResponseServer"/>, содержащий информацию о заказе или ошибке.</returns>
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

    /// <summary>
    /// Получает все заказы, связанные с указанным идентификатором пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Результат операции в виде <see cref="ResponseServer"/>, содержащий список заказов пользователя или ошибку.</returns>
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

    /// <summary>
    /// Обновляет заголовок заказа по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор заказа.</param>
    /// <param name="orderHeaderUpdateDto">Объект, содержащий данные для обновления заголовка заказа.</param>
    /// <returns>Результат операции в виде <see cref="ResponseServer"/>, указывающий на успешное обновление или ошибку.</returns>
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