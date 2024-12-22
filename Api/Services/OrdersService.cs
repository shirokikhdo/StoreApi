using Api.Common;
using Api.Data;
using Api.ModelDto;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

/// <summary>
/// Сервис для управления заказами, включая создание, получение и обновление заказов.
/// </summary>
public class OrdersService
{
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="OrdersService"/> с заданным контекстом базы данных.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных, используемый для доступа к данным заказов.</param>
    public OrdersService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Асинхронно создает новый заказ на основе предоставленных данных.
    /// </summary>
    /// <param name="orderHeaderCreateDto">Объект, содержащий данные для создания заказа.</param>
    /// <returns>Созданный объект заказа <see cref="OrderHeader"/>.</returns>
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

    /// <summary>
    /// Асинхронно получает заказ по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор заказа.</param>
    /// <returns>Объект заказа <see cref="OrderHeader"/>, если найден; в противном случае null.</returns>
    public async Task<OrderHeader> GetOrderByIdAsync(int id) =>
        await _dbContext.OrderHeaders
            .Include(x => x.OrderDetailItems)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.OrderHeaderId == id);

    /// <summary>
    /// Асинхронно получает список заказов для указанного пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Список объектов заказов <see cref="IEnumerable{OrderHeader}"/>.</returns>
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

    /// <summary>
    /// Асинхронно обновляет заголовок заказа по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор заказа, который необходимо обновить.</param>
    /// <param name="orderHeaderUpdateDto">Объект, содержащий данные для обновления заказа.</param>
    /// <returns>true, если обновление прошло успешно; иначе false.</returns>
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