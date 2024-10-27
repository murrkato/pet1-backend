using Microsoft.EntityFrameworkCore;
using pet1_backend.Data.Models;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasMany(u => u.Roles)
        .WithMany(r => r.Users)
        .UsingEntity<UserRole>(
          l => l.HasOne<Role>().WithMany().HasForeignKey(l => l.RoleId),
          r => r.HasOne<User>().WithMany().HasForeignKey(u => u.UserId)
        );
    }
}