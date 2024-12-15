using System.Net;
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
    public async Task<IActionResult> GetProducts()
    {
        return Ok(new ResponseServer
        {
            StatusCode = HttpStatusCode.OK,
            Result = await _dbContext.Products.ToListAsync()
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetProductById(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new ResponseServer
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                ErrorMessages = {"Неверный Id"}
            });
        }

        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        
        if (product is null)
            return NotFound(new ResponseServer
            {
                StatusCode = HttpStatusCode.NotFound,
                IsSuccess = false,
                ErrorMessages = {"Продукт по указанному Id не найден"}
            });
        
        return Ok(new ResponseServer
        {
            StatusCode = HttpStatusCode.OK,
            Result = product
        });
    }
}