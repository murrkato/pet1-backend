using pet1_backend.Data.Models;
using pet1_backend.Dtos.Account;
using pet1_backend.Dtos.Account.Client;
using pet1_backend.Services;

namespace pet1_backend.Mapping
{
  public static class UserMapping
  {
    public static User ToEntity(this UserRegDto user, IPasswordService _passwordService)
    {
      var newUser = new User {
        Username = user.Username,
        Email = user.Email,
        PasswordHash = "",
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now
      };

      HashedDataDto hashedStr = _passwordService.HashPassword(user.Password, null);
      newUser.PasswordHash = hashedStr.Value;
      newUser.Salt = hashedStr.Salt;

      return newUser;
    }

    public static UserRegRespDto ToDto(this User user)
    {
      return new UserRegRespDto
      {
        Id = user.Id,
        Username = user.Username,
        Email = user.Email
      };
    }
  }
}