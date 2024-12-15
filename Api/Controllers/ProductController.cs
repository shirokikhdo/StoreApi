using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

public class ProductController : StoreController
{
    public ProductController(AppDbContext dbContext) : base(dbContext)
    {

    }

    [HttpGet]
    public async Task<IActionResult> GetProducts() =>
        Ok(await _dbContext.Products.ToListAsync());
}