using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using pet1_backend.Data;
using pet1_backend.Helpers;
using pet1_backend.Services;
using pet1_backend.Validation;

namespace pet1_backend.Middlewares;
public class ValidateJwtToken
{
  private readonly RequestDelegate _next;
  private readonly IConfiguration _configuration;

  public ValidateJwtToken(
    RequestDelegate next,
    IConfiguration configuration
    )
  {
    _next = next;
    _configuration = configuration;
  }

  public async Task InvokeAsync(HttpContext context, AppDbContext _dbContext, IJWTTokenService _jwtTokenService)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
    var negResp = new NegResponse {
      Error = ValidationMessages._errorStatuses[401],
      ErrMessage = ValidationMessages._errMessages[401],
      Service = "authBearer",
    };

    if (token == null)
    {
      await SetAuthErr(context, negResp);
      return;
    }

    var storedToken = await _dbContext.AccessTokens.FirstOrDefaultAsync(t => t.Token == token[1]);

    if (storedToken == null)
    {
      await SetAuthErr(context, negResp);
      return;
    }

    var validToken = _jwtTokenService.VerifyTokenHash(token[1], storedToken.Token);

    if (validToken == false)
    {
      await SetAuthErr(context, negResp);
      return;
    }
    
    tokenHandler.ValidateToken(token[1], new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(storedToken.Salt),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    }, out SecurityToken validatedToken);

    var jwtToken = (JwtSecurityToken)validatedToken;
    var userId = jwtToken.Claims.First(c => c.Type == "NameIdentifier").Value;
    var userRole = jwtToken.Claims.Where(r => r.Type == "Role").Select(r => r.Value).ToList();

    Console.WriteLine($"{userId} and {userRole}");

    await _next(context);
  }

  private static async Task SetAuthErr(HttpContext context, NegResponse negResp)
  {
      var json = JsonSerializer.Serialize(negResp);

      context.Response.StatusCode = 401;
      context.Response.ContentType = "application/json";
      
      await context.Response.WriteAsync(json);
  }
}