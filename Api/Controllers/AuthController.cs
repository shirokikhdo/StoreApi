using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.Controllers;

public class AuthController : StoreController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthController(
        AppDbContext dbContext,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager) 
        : base(dbContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
}