using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

/// <summary>
/// Сервис для управления корзинами покупок, включая создание, обновление и получение корзин.
/// </summary>
public class ShoppingCartService
{
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ShoppingCartService"/> с заданным контекстом базы данных.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных, используемый для доступа к данным корзин покупок.</param>
    public ShoppingCartService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Асинхронно создает новую корзину покупок для указанного пользователя и добавляет в нее товар.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя, для которого создается корзина.</param>
    /// <param name="productId">Идентификатор товара, который добавляется в корзину.</param>
    /// <param name="quantity">Количество товара, добавляемого в корзину.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    public async Task CreateNewCartAsync(
        string userId, 
        int productId,
        int quantity)
    {
        var newCart = new ShoppingCart
        {
            UserId = userId
        };

        await _dbContext.ShoppingCarts.AddAsync(newCart);
        await _dbContext.SaveChangesAsync();

        var cartItem = new CartItem
        {
            ProductId = productId,
            Quantity = quantity,
            ShoppingCartId = newCart.Id,
            Product = null
        };

        await _dbContext.CartItems.AddAsync(cartItem);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Асинхронно обновляет существующую корзину покупок, добавляя или изменяя количество товара.
    /// </summary>
    /// <param name="shoppingCart">Корзина покупок, которую необходимо обновить.</param>
    /// <param name="productId">Идентификатор товара, который необходимо обновить.</param>
    /// <param name="newQuantity">Новое количество товара. Если 0 или меньше, товар будет удален из корзины.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    public async Task UpdateExistingCartAsync(
        ShoppingCart shoppingCart,
        int productId,
        int newQuantity)
    {
        var cartItem =  shoppingCart.CartItems
            .FirstOrDefault(x=>x.ProductId == productId);

        if (cartItem is null && newQuantity > 0)
        {
            var newCartItem = new CartItem
            {
                ProductId = productId,
                Quantity = newQuantity,
                ShoppingCartId = shoppingCart.Id,
                Product = null
            };

            await _dbContext.CartItems.AddAsync(newCartItem);
        }
        else if(cartItem != null)
        {
            var updatedQuantity = cartItem.Quantity + newQuantity;
            if (newQuantity == 0 || updatedQuantity <= 0)
            {
                _dbContext.CartItems.Remove(cartItem);
                if (shoppingCart.CartItems.Count == 1)
                    _dbContext.ShoppingCarts.Remove(shoppingCart);
            }
            else
                cartItem.Quantity = updatedQuantity;
        }

        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Асинхронно получает корзину покупок для указанного пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя, для которого необходимо получить корзину.</param>
    /// <returns>Корзина покупок <see cref="ShoppingCart"/>, если найдена; иначе новая пустая корзина.</returns>
    public async Task<ShoppingCart> GetShoppingCartAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return new ShoppingCart();
        }

        var shoppingCart = await _dbContext.ShoppingCarts
            .Include(x => x.CartItems)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x=>x.UserId == userId);

        if (shoppingCart != null
            && shoppingCart.CartItems != null)
        {
            shoppingCart.TotalAmount = shoppingCart.CartItems
                .Sum(x => x.Quantity * x.Product.Price);
        }
        return shoppingCart;
    }
}