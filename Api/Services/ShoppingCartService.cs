using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class ShoppingCartService
{
    private readonly AppDbContext _dbContext;

    public ShoppingCartService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

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