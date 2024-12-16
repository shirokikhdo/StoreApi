using System.Net;
using Api.Common;
using Api.Data;
using Api.ModelDto;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

public class AuthController : StoreController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public AuthController(
        AppDbContext dbContext,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        JwtTokenGenerator jwtTokenGenerator) 
        : base(dbContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [HttpPost]
    public async Task<ActionResult<ResponseServer>> Register(
        [FromBody] RegisterRequestDto registerRequestDto)
    {
        if (registerRequestDto is null)
            return BadRequest(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = {"Некорректная модель запроса"}
            });

        var user = await _dbContext.AppUsers.FirstOrDefaultAsync(
            x => string.Equals(
                x.UserName.ToLower(), 
                registerRequestDto.Username.ToLower()));

        if(user != null)
            return BadRequest(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = { "Такой пользователь есть" }
            });

        var newUser = new AppUser
        {
            UserName = registerRequestDto.Username,
            Email = registerRequestDto.Email,
            NormalizedEmail = registerRequestDto.Email.ToUpper(),
            FirstName = registerRequestDto.Username
        };

        var userResult = await _userManager.CreateAsync(
            newUser, 
            registerRequestDto.Password);

        if(!userResult.Succeeded)
            return BadRequest(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = { "Ошибка регистрации" }
            });

        var newUserRole = registerRequestDto.Role.Equals(
            SharedData.Roles.Admin, StringComparison.OrdinalIgnoreCase)
            ? SharedData.Roles.Admin
            : SharedData.Roles.Consumer;

        await _userManager.AddToRoleAsync(newUser, newUserRole);

        return Ok(new ResponseServer
        {
            StatusCode = HttpStatusCode.OK,
            Result = "Регистрация завершена"
        });
    }

    [HttpPost]
    public async Task<ActionResult<ResponseServer>> Login(
        [FromBody] LoginRequestDto loginRequestDto)
    {
        var user = await _dbContext.AppUsers
            .FirstOrDefaultAsync(x=>string.Equals(
                x.Email.ToLower(), 
                loginRequestDto.Email.ToLower()));

        if(user is null 
           || !await _userManager.CheckPasswordAsync(user, loginRequestDto.Password))
            return BadRequest(new ResponseServer
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = { "Такого пользователя нет" }
            });

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenGenerator.GenerateJwtToken(user, roles);

        return Ok(new ResponseServer
        {
            StatusCode = HttpStatusCode.OK,
            Result = new LoginResponseDto
            {
                Email = user.Email,
                Token = token
            }
        });
    }
}