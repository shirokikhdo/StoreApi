using System.Net;
using Api.Data;
using Api.ModelDto;
using Api.Models;
using Api.Services.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

/// <summary>
/// Контроллер для управления продуктами в магазине.
/// </summary>
public class ProductController : StoreController
{
    private readonly IFileStorageService _fileStorage;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ProductController"/> с заданным контекстом базы данных и сервисом хранения файлов.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных для доступа к данным продуктов.</param>
    /// <param name="fileStorage">Сервис для загрузки и хранения файлов изображений продуктов.</param>
    public ProductController(
        AppDbContext dbContext,
        IFileStorageService fileStorage) 
        : base(dbContext)
    {
        _fileStorage = fileStorage;
    }

    /// <summary>
    /// Получает список всех продуктов.
    /// </summary>
    /// <returns>Возвращает результат операции в виде <see cref="ResponseServer"/>, содержащий список продуктов.</returns>
    [HttpGet]
    public async Task<ActionResult<ResponseServer>> GetProducts()
    {
        return Ok(new ResponseServer
        {
            StatusCode = HttpStatusCode.OK,
            Result = await _dbContext.Products.ToListAsync()
        });
    }

    /// <summary>
    /// Получает продукт по указанному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор продукта.</param>
    /// <returns>Возвращает результат операции в виде <see cref="ResponseServer"/>, содержащий продукт или сообщение об ошибке.</returns>
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

    /// <summary>
    /// Создает новый продукт.
    /// </summary>
    /// <param name="productCreateDto">Данные для создания продукта.</param>
    /// <returns>Возвращает результат операции в виде <see cref="ResponseServer"/>, содержащий созданный продукт или сообщение об ошибке.</returns>
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

    /// <summary>
    /// Обновляет существующий продукт.
    /// </summary>
    /// <param name="id">Идентификатор продукта, который нужно обновить.</param>
    /// <param name="productUpdateDto">Данные для обновления продукта.</param>
    /// <returns>Возвращает результат операции в виде <see cref="ResponseServer"/>, содержащий обновленный продукт или сообщение об ошибке.</returns>
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

    /// <summary>
    /// Удаляет продукт по заданному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор продукта, который необходимо удалить.</param>
    /// <returns>Возвращает результат операции в виде <see cref="ResponseServer"/> с информацией о статусе.</returns>
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

            await _fileStorage.RemoveFileAsync(
                product.Image.Split('/').Last());
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

    /// <summary>
    /// Получает список продуктов с поддержкой пагинации.
    /// </summary>
    /// <param name="skip">Количество пропущенных записей (для пагинации).</param>
    /// <param name="take">Количество записей, которые необходимо вернуть.</param>
    /// <returns>Возвращает список продуктов в виде <see cref="ResponseServer"/> с информацией о статусе.</returns>
    [HttpGet]
    public async Task<ActionResult<ResponseServer>> FetchProductsWithPagination(
        int skip = 0, int take = 5)
    {
        var products = await _dbContext.Products
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return Ok(new ResponseServer
        {
            StatusCode = HttpStatusCode.OK,
            Result = products
        });
    }
}