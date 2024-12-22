using Api.Data;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// Базовый контроллер для управления операциями в магазине.
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StoreController : ControllerBase
    {
        /// <summary>
        /// Контекст базы данных для доступа к данным магазина.
        /// </summary>
        protected readonly AppDbContext _dbContext;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="StoreController"/> с заданным контекстом базы данных.
        /// </summary>
        /// <param name="dbContext">Контекст базы данных для доступа к данным.</param>
        public StoreController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}