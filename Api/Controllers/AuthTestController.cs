using System.Net;
using Api.Common;
using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Контроллер для тестирования аутентификации и авторизации пользователей.
/// </summary>
public class AuthTestController : StoreController
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AuthTestController"/> с заданным контекстом базы данных.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных для доступа к данным.</param>
    public AuthTestController(AppDbContext dbContext) 
        : base(dbContext)
    {

    }

    /// <summary>
    /// Тестовый метод, доступный для всех пользователей.
    /// </summary>
    /// <returns>Результат операции в виде <see cref="ResponseServer"/> с сообщением "Test1: Для всех".</returns>
    [HttpGet]
    public ActionResult<ResponseServer> Test1()
    {
        return Ok(new ResponseServer
        {
            StatusCode = HttpStatusCode.OK,
            Result = "Test1: Для всех"
        });
    }

    /// <summary>
    /// Тестовый метод, доступный только для авторизированных пользователей.
    /// </summary>
    /// <returns>Результат операции в виде <see cref="ResponseServer"/> с сообщением "Test2: Для авторизированных пользователей".</returns>
    [HttpGet]
    [Authorize]
    public ActionResult<ResponseServer> Test2()
    {
        return Ok(new ResponseServer
        {
            StatusCode = HttpStatusCode.OK,
            Result = "Test2: Для авторизированных пользователей"
        });
    }

    /// <summary>
    /// Тестовый метод, доступный только для авторизированных пользователей с ролью Consumer.
    /// </summary>
    /// <returns>Результат операции в виде <see cref="ResponseServer"/> с сообщением "Test3: Для авторизированных пользователей с ролью Consumer".</returns>
    [HttpGet]
    [Authorize(Roles = SharedData.Roles.Consumer)]
    public ActionResult<ResponseServer> Test3()
    {
        return Ok(new ResponseServer
        {
            StatusCode = HttpStatusCode.OK,
            Result = "Test3: Для авторизированных пользователей с ролью Consumer"
        });
    }

    /// <summary>
    /// Тестовый метод, доступный только для авторизированных пользователей с ролью Admin.
    /// </summary>
    /// <returns>Результат операции в виде <see cref="ResponseServer"/> с сообщением "Test4: Для авторизированных пользователей с ролью Admin".</returns>
    [HttpGet]
    [Authorize(Roles = SharedData.Roles.Admin)]
    public ActionResult<ResponseServer> Test4()
    {
        return Ok(new ResponseServer
        {
            StatusCode = HttpStatusCode.OK,
            Result = "Test4: Для авторизированных пользователей с ролью Admin"
        });
    }
}