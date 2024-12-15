using System.Net;
using Api.Data;
using Api.ModelDto;
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

    [HttpGet("{id}", Name = nameof(GetProductById))]
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

    [HttpPost]
    public async Task<ActionResult<ResponseServer>> CreateProduct(
        [FromBody] ProductCreateDto productCreateDto)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if (productCreateDto.Image is null
                    || productCreateDto.Image.Length == 0)
                {
                    return BadRequest(new ResponseServer
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        ErrorMessages = {"Image не может быть пустым"}
                    });
                }

                var item = new Product
                {
                    Name = productCreateDto.Name,
                    Description = productCreateDto.Description,
                    SpecialTag = productCreateDto.SpecialTag,
                    Category = productCreateDto.Category,
                    Price = productCreateDto.Price,
                    Image = "https://placehold.co/250"
                };

                await _dbContext.Products.AddAsync(item);
                await _dbContext.SaveChangesAsync();

                var response = new ResponseServer
                {
                    StatusCode = HttpStatusCode.Created,
                    Result = item
                };
                return CreatedAtRoute(nameof(GetProductById), new {id = item.Id}, response);
            }

            return BadRequest(new ResponseServer
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                ErrorMessages = {"Модель данных не подходит"}
            });
        }
        catch(Exception e)
        {
            return BadRequest(new ResponseServer
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                ErrorMessages = { "Что-то пошло не так", e.Message }
            });
        }
    }
}