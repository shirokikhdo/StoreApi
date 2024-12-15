using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ProductController : StoreController
{
    [HttpGet]
    public async Task<string> Get123() => await Task.FromResult<string>("Пртвет");
}