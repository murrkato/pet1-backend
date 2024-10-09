using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using pet1_backend.Data.Models;
using pet1_backend.Dtos.Account.Client;

namespace pet1_backend.Services
{
  public interface IPasswordService
  {
    HashedDataDto HashPassword(string password, byte[]? salt);
    bool VerifyPasswordHash(string password, User user);
  }
  public class PasswordService : IPasswordService
  {
    public HashedDataDto HashPassword(string password, byte[]? salt)
    {
        if (salt == null)
        {
          salt = RandomNumberGenerator.GetBytes(128 / 8);
        }

        string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        password: password!,
        salt: salt,
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: 100000,
        numBytesRequested: 256 / 8));

        return new HashedDataDto {
          Value = hashedPassword,
          Salt = salt
        };
    }

    public bool VerifyPasswordHash(string password, User user)
    {
      var hashed = HashPassword(password, user.Salt); 
      return hashed.Value == user.PasswordHash;
    }
  }
}