using JwtAuthorizeTest.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthorizeTest.Server.Data;

public class ApplicationDbContext:DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) 
    {
        
    }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x=> x.UserName).IsUnique();
        base.OnModelCreating(modelBuilder);
    }

}
