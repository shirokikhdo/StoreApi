using Api.Data;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StoreController : ControllerBase
    {
        protected readonly AppDbContext _dbContext;

        public StoreController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}