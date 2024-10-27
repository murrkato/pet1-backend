using Microsoft.EntityFrameworkCore;
using pet1_backend.Data.Models;

namespace pet1_backend.Data {
  public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
  {
    public DbSet<User> Users {get; set;}
    public DbSet<AccessToken> AccessTokens {get; set;}
    public DbSet<Role> Roles {get; set;}
    public DbSet<UserRole> UserRoles {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguraion());
        
        base.OnModelCreating(modelBuilder);
    }
    }
}