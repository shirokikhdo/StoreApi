using System.Net;
using Api.Data;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

public class ShoppingCartController : StoreController
{
    private readonly ShoppingCartService _shoppingCartService;

    public ShoppingCartController(
        AppDbContext dbContext,
        ShoppingCartService shoppingCartService) 
        : base(dbContext)
    {
        _shoppingCartService = shoppingCartService;
    }

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