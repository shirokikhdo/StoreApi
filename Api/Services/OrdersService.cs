using System.Security.Cryptography.Xml;
using Api.Common;
using Api.Data;
using Api.ModelDto;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class OrdersService
{
    private readonly AppDbContext _dbContext;

    public OrdersService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OrderHeader> CreateOrderAsync(
        OrderHeaderCreateDto orderHeaderCreateDto)
    {
        var order = new OrderHeader
        {
            AppUserId = orderHeaderCreateDto.AppUserId,
            CustomerName = orderHeaderCreateDto.CustomerName,
            CustomerEmail = orderHeaderCreateDto.CustomerEmail,
            TotalOrderAmount = orderHeaderCreateDto.OrderTotalAmount,
            TotalCount = orderHeaderCreateDto.TotalCount,
            OrderDateTime = DateTime.UtcNow,
            Status = string.IsNullOrEmpty(orderHeaderCreateDto.Status)
                ? SharedData.OrderStatus.Pending
                : orderHeaderCreateDto.Status
        };

        await _dbContext.OrderHeaders.AddAsync(order);
        await _dbContext.SaveChangesAsync();

        foreach (var orderDetailsDto in orderHeaderCreateDto.OrderDetailsDto)
        {
            var orderDetails = new OrderDetails
            {
                OrderHeaderId = order.OrderHeaderId,
                ProductId = orderDetailsDto.ProductId,
                Quantity = orderDetailsDto.Quantity,
                ItemName = orderDetailsDto.ItemName,
                Price = orderDetailsDto.Price,
            };

            await _dbContext.OrderDetails.AddAsync(orderDetails);
        }
        await _dbContext.SaveChangesAsync();

        return order;
    }

    public async Task<OrderHeader> GetOrderByIdAsync(int id) =>
        await _dbContext.OrderHeaders
            .Include(x => x.OrderDetailItems)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.OrderHeaderId == id);

    public async Task<IEnumerable<OrderHeader>> GetOrdersByUserIdAsync(string userId)
    {
        var query = _dbContext.OrderHeaders
            .Include(x => x.OrderDetailItems)
            .ThenInclude(x => x.Product)
            .OrderByDescending(x=>x.AppUserId);

        if(!string.IsNullOrEmpty(userId))
            return await query
                .Where(x=>x.AppUserId == userId)
                .ToListAsync();

        return await query.ToListAsync();
    }

    public async Task<bool> UpdateOrderHeaderAsync(
        int id, OrderHeaderUpdateDto orderHeaderUpdateDto)
    {
        if (orderHeaderUpdateDto == null
            || orderHeaderUpdateDto.OrderHeaderId != id)
            return false;

        var order = await _dbContext.OrderHeaders
            .FirstOrDefaultAsync(x => x.OrderHeaderId == id);

        if (order is null)
            return false;

        if (!string.IsNullOrEmpty(orderHeaderUpdateDto.CustomerName))
            order.CustomerName = orderHeaderUpdateDto.CustomerName;

        if (!string.IsNullOrEmpty(orderHeaderUpdateDto.CustomerEmail))
            order.CustomerEmail = orderHeaderUpdateDto.CustomerEmail;

        if (!string.IsNullOrEmpty(orderHeaderUpdateDto.Status))
            order.Status = orderHeaderUpdateDto.Status;

        await _dbContext.SaveChangesAsync();
        return true;
    }
}