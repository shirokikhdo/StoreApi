using System.Net;
using Api.Common;
using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class AuthTestController : StoreController
{
    public AuthTestController(AppDbContext dbContext) 
        : base(dbContext)
    {

    }

    [HttpGet]
    public ActionResult<ResponseServer> Test1()
    {
        return Ok(new ResponseServer
        {
            StatusCode = HttpStatusCode.OK,
            Result = "Test1: Для всех"
        });
    }

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