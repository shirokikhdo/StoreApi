using System.Net;
using Api.Data;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

/// <summary>
/// Контроллер для управления корзиной покупок пользователей.
/// </summary>
public class ShoppingCartController : StoreController
{
    private readonly ShoppingCartService _shoppingCartService;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ShoppingCartController"/> с заданным контекстом базы данных и сервисом корзины покупок.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных для доступа к данным.</param>
    /// <param name="shoppingCartService">Сервис для работы с корзиной покупок.</param>
    public ShoppingCartController(
        AppDbContext dbContext,
        ShoppingCartService shoppingCartService) 
        : base(dbContext)
    {
        _shoppingCartService = shoppingCartService;
    }

    /// <summary>
    /// Добавляет или обновляет товар в корзине покупок пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя, чья корзина будет обновлена.</param>
    /// <param name="productId">Идентификатор продукта, который нужно добавить или обновить.</param>
    /// <param name="updateQuantity">Количество товара для добавления или обновления в корзине.</param>
    /// <returns>Возвращает результат операции в виде <see cref="ResponseServer"/>, содержащий сообщение об успешном обновлении корзины или ошибке.</returns>
    [HttpGet]
    public async Task<ActionResult<ResponseServer>> AppendOrUpdateItemInCart(
        string userId,
        int productId, 
        int updateQuantity)
    {
        var product = await _dbContext.Products.FindAsync(productId);
        if (product is null)
            return BadRequest(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = {"Такого товара не найдено"}
            });

        var shoppingCart = _dbContext.ShoppingCarts
            .Include(x=>x.CartItems)
            .FirstOrDefault(x => x.UserId == userId);
        if (shoppingCart is null && updateQuantity > 0)
            await _shoppingCartService.CreateNewCartAsync(userId, productId, updateQuantity);
        else if (shoppingCart != null)
            await _shoppingCartService.UpdateExistingCartAsync(shoppingCart, productId, updateQuantity);
        return Ok(new ResponseServer
        {
            StatusCode = HttpStatusCode.OK,
            Result = "Корзина обновлена"
        });
    }

    /// <summary>
    /// Получает корзину покупок пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя, чью корзину нужно получить.</param>
    /// <returns>Возвращает результат операции в виде <see cref="ResponseServer"/>, содержащий корзину покупок или сообщение об ошибке.</returns>
    [HttpGet]
    public async Task<ActionResult<ResponseServer>> GetShoppingCart(string userId)
    {
        try
        {
            var shoppingCart = await _shoppingCartService.GetShoppingCartAsync(userId);
            return Ok(new ResponseServer
            {
                StatusCode = HttpStatusCode.OK,
                Result = shoppingCart
            });
        }
        catch (Exception e)
        {
            return BadRequest(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = {"Ошибка получения корзины", e.Message}
            });
        }
    }
}