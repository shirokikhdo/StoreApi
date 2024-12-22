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

/// <summary>
/// Контроллер для аутентификации пользователей, включая регистрацию и вход в систему.
/// </summary>
public class AuthController : StoreController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AuthController"/> с заданными зависимостями.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных для доступа к данным пользователей.</param>
    /// <param name="userManager">Менеджер пользователей для управления операциями с пользователями.</param>
    /// <param name="roleManager">Менеджер ролей для управления ролями пользователей.</param>
    /// <param name="jwtTokenGenerator">Генератор JWT-токенов для аутентификации.</param>
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

    /// <summary>
    /// Регистрация нового пользователя.
    /// </summary>
    /// <param name="registerRequestDto">Модель запроса для регистрации пользователя, содержащая данные пользователя.</param>
    /// <returns>Результат операции регистрации в виде <see cref="ResponseServer"/>.</returns>
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

    /// <summary>
    /// Вход пользователя в систему.
    /// </summary>
    /// <param name="loginRequestDto">Модель запроса для входа пользователя, содержащая данные для аутентификации.</param>
    /// <returns>Результат операции входа в систему в виде <see cref="ResponseServer"/> с JWT-токеном.</returns>
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