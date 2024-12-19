using System.Net;
using Api.Data;
using Api.ModelDto;
using Api.Models;
using Api.Services.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

public class ProductController : StoreController
{
    private readonly IFileStorageService _fileStorage;

    public ProductController(
        AppDbContext dbContext,
        IFileStorageService fileStorage) 
        : base(dbContext)
    {
        _fileStorage = fileStorage;
    }

    [HttpGet]
    public async Task<ActionResult<ResponseServer>> GetProducts()
    {
        return Ok(new ResponseServer
        {
            StatusCode = HttpStatusCode.OK,
            Result = await _dbContext.Products.ToListAsync()
        });
    }

    [HttpGet("{id}", Name = nameof(GetProductById))]
    public async Task<ActionResult<ResponseServer>> GetProductById(int id)
    {
        if (id <= 0)
            return BadRequest(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = { "Неверный Id" }
            });

        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (product is null)
            return NotFound(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
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
        [FromForm] ProductCreateDto productCreateDto)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if (productCreateDto.Image is null
                    || productCreateDto.Image.Length == 0)
                    return BadRequest(new ResponseServer
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = { "Image не может быть пустым" }
                    });

                var item = new Product
                {
                    Name = productCreateDto.Name,
                    Description = productCreateDto.Description,
                    SpecialTag = productCreateDto.SpecialTag,
                    Category = productCreateDto.Category,
                    Price = productCreateDto.Price,
                    Image = await _fileStorage.UploadFileAsync(productCreateDto.Image)
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
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = {"Модель данных не подходит"}
            });
        }
        catch (Exception e)
        {
            return BadRequest(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = {"Что-то пошло не так", e.Message}
            });
        }
    }

    [HttpPut]
    public async Task<ActionResult<ResponseServer>> UpdateProduct(
        int id,
        [FromForm] ProductUpdateDto productUpdateDto)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if (productUpdateDto is null
                    || productUpdateDto.Id != id)
                    return BadRequest(new ResponseServer
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = {"Несоответствие модели данных"}
                    });

                var productFromDb = await _dbContext.Products.FindAsync(id);
                    
                if(productFromDb is null)
                    return NotFound(new ResponseServer
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = { "Продукт с таким Id не найден" }
                    });

                productFromDb.Name = productUpdateDto.Name;
                productFromDb.Description= productUpdateDto.Description;
                if(!string.IsNullOrEmpty(productUpdateDto.SpecialTag))
                    productFromDb.SpecialTag = productUpdateDto.SpecialTag;
                if (!string.IsNullOrEmpty(productUpdateDto.Category))
                    productFromDb.Category = productUpdateDto.Category;
                productFromDb.Price = productUpdateDto.Price;
                if (productUpdateDto.Image != null
                    && productUpdateDto.Image.Length > 0)
                {
                    await _fileStorage.RemoveFileAsync(
                        productFromDb.Image.Split('/').Last());
                    productFromDb.Image = await _fileStorage.UploadFileAsync(productUpdateDto.Image);
                }

                _dbContext.Products.Update(productFromDb);
                await _dbContext.SaveChangesAsync();

                return Ok(new ResponseServer
                {
                    StatusCode = HttpStatusCode.OK,
                    Result = productFromDb
                });
            }

            return BadRequest(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = { "Модель не соответствует" }
            });
        }
        catch (Exception e)
        {
            return BadRequest(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = { "Что-то пошло не так", e.Message }
            });
        }
    }

    [HttpDelete]
    public async Task<ActionResult<ResponseServer>> RemoveProductById(int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest(new ResponseServer
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = {"Неверный Id"}
                });

            var product = await _dbContext.Products.FindAsync(id);

            if (product is null)
                return NotFound(new ResponseServer
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = {"Продукт по указанному Id не найден"}
                });

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            return Ok(new ResponseServer
            {
                StatusCode = HttpStatusCode.NoContent,
            });
        }
        catch (Exception e)
        {
            return BadRequest(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = { "Что-то пошло не так", e.Message }
            });
        }
    }
}