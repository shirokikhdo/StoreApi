using Api.Models;
using Api.Seed;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

/// <summary>
/// Контекст базы данных приложения, наследующий от <c>IdentityDbContext</c>.
/// Используется для взаимодействия с базой данных и управления сущностями.
/// </summary>
public class AppDbContext : IdentityDbContext
{
    /// <summary>
    /// Получает или задает коллекцию пользователей приложения.
    /// </summary>
    public DbSet<AppUser> AppUsers { get; set; }
    /// <summary>
    /// Получает или задает коллекцию продуктов.
    /// </summary>
    public DbSet<Product> Products { get; set; }
    /// <summary>
    /// Получает или задает коллекцию элементов корзины покупок.
    /// </summary>
    public DbSet<CartItem> CartItems { get; set; }
    /// <summary>
    /// Получает или задает коллекцию корзин покупок.
    /// </summary>
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    /// <summary>
    /// Получает или задает коллекцию деталей заказов.
    /// </summary>
    public DbSet<OrderDetails> OrderDetails { get; set; }
    /// <summary>
    /// Получает или задает коллекцию заголовков заказов.
    /// </summary>
    public DbSet<OrderHeader> OrderHeaders { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <c>AppDbContext</c>.
    /// </summary>
    /// <param name="options">Параметры контекста базы данных.</param>
    public AppDbContext(DbContextOptions options) 
        : base(options)
    {
        
    }

    /// <summary>
    /// Конфигурирует модель базы данных при ее создании.
    /// </summary>
    /// <param name="builder">Объект для построения модели.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Product>().HasData(FakeProductGenerator.GenerateProductList(10));
    }
}