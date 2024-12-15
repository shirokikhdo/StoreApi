using Api.Models;
using Api.Seed;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class AppDbContext : IdentityDbContext
{
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Product> Products { get; set; }

    public AppDbContext(DbContextOptions options) 
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Product>().HasData(FakeProductGenerator.GenerateProductList());
    }
}