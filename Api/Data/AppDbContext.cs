using Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class AppDbContext : IdentityDbContext
{
    public DbSet<AppUser> AppUsers { get; set; }

    public AppDbContext(DbContextOptions options) 
        : base(options)
    {
        
    }
}