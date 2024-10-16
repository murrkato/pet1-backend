using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using pet1_backend.Data;
using pet1_backend.Data.Models;
using pet1_backend.Dtos.Account.Client;

namespace pet1_backend.Services
{
  public interface IJWTTokenService
  {
    string GenerateToken(string userId, string username);
    HashedDataDto HashToken(string token);
    bool VerifyTokenHash(string token, string hashedToken);
    public Task<AccessToken> CreateToken(User user, string accessToken);
  }
  public class JwtTokenService : IJWTTokenService
  {
    public readonly IConfiguration _configuration;
    public AppDbContext _context;

    public JwtTokenService(IConfiguration configuration, AppDbContext context)
    {
      _configuration = configuration;
      _context = context;
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
        Salt = salt,
        Expires = DateTime.UtcNow.AddHours(24),
      };
    }

    public bool VerifyTokenHash(string token, string hashedToken)
    {
      HashedDataDto hashedInputToken = HashToken(token);
      return hashedInputToken.Value == hashedToken;
    }


    //TODO: add delete token

    public async Task<AccessToken> CreateToken(User user, string accessToken)
    {
      var oldToken = await _context.AccessTokens.FirstOrDefaultAsync(token => token.UserId == user.Id);

      if (oldToken != null)
      {
        _context.AccessTokens.Remove(oldToken);
        await _context.SaveChangesAsync();
      }

      HashedDataDto hashedToken = HashToken(accessToken);
      var token = new AccessToken
      {
        Token = hashedToken.Value,
        Salt = hashedToken.Salt,
        CreatedAt = DateTime.UtcNow,
        ExpiresAt = hashedToken.Expires ?? DateTime.UtcNow.AddHours(1),
        UserId = user.Id
      };

      _context.AccessTokens.Add(token);
      await _context.SaveChangesAsync();

      return token;
    } 
  }
}