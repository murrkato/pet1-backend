using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using pet1_backend.Dtos.Account.Client;

namespace pet1_backend.Services
{
  public interface IJWTTokenService
  {
    string GenerateToken(string userId, string username);
    HashedDataDto HashToken(string token);
    bool VerifyTokenHash(string token, string hashedToken);
  }
  public class JwtTokenService : IJWTTokenService
  {
    public readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public string GenerateToken(string userId, string username)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var storedKey = _configuration["Jwt:Key"];

      if (storedKey != null)
      {
        var key = Encoding.ASCII.GetBytes(storedKey);
        // var key = Convert.FromBase64String(storedKey);
        var claims = new[] {
          new Claim(ClaimTypes.Name, username),
          new Claim(ClaimTypes.NameIdentifier, userId)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
          Subject = new ClaimsIdentity(claims),
          Expires = DateTime.UtcNow.AddHours(24),
          SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature
          )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
      }

      return "";
    }

    public HashedDataDto HashToken(string token)
    {
      using SHA256 sha256Hash = SHA256.Create();
      byte[] salt = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(token));
      string hashedToken = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        password: token!,
        salt: salt,
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: 100000,
        numBytesRequested: 256 / 8));;

      return new HashedDataDto {
        Value = hashedToken,
        Salt = salt
      };
    }

    public bool VerifyTokenHash(string token, string hashedToken)
    {
      HashedDataDto hashedInputToken = HashToken(token);
      return hashedInputToken.Value == hashedToken;
    }
  }
}