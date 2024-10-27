using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pet1_backend.Data.Enums;
using pet1_backend.Data.Models;

public class RoleConfiguraion : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(ur => ur.Id);

        var roles = Enum
          .GetValues<RoleEnum>()
          .Select(r => new Role
          {
            Id = (int)r,
            Name = r.ToString()
          });

        builder.HasData(roles);
    }
}