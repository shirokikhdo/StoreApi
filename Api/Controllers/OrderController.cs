using Api.Data;
using Api.Services;

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
}