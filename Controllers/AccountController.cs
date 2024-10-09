using Microsoft.AspNetCore.Mvc;
using pet1_backend.Dtos.Account;
using pet1_backend.Helpers;
using pet1_backend.Services;
using pet1_backend.Validation;
using pet1_backend.Mapping;
using pet1_backend.Data.Models;
using pet1_backend.Data;
using Microsoft.EntityFrameworkCore;
using pet1_backend.Dtos.Account.Client;

namespace pet1_backend.Controllers
{
  [ApiController]
  public class AccountController : ControllerBase
  {
    private readonly AppDbContext _context;
    private readonly IUserValidationService _userValidationService;
    private readonly IPasswordService _passwordService;
    private readonly IJWTTokenService _jwtTokenService;

    public AccountController (
        AppDbContext context,
        IUserValidationService userValidationService,
        IPasswordService passwordService,
        IJWTTokenService JWTTokenService
      )
    {
      _context = context;
      _userValidationService = userValidationService;
      _passwordService = passwordService;
      _jwtTokenService = JWTTokenService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Authenticate([FromBody] UserRegDto user)
    {
      var service = "Authenticate";
      var negResp = new NegResponse
      {
        Error = "",
        ErrMessage = "",
        Service = service,
      };

      if (user == null)
      {
        negResp.Error = ValidationMessages._errorStatuses[400];
        negResp.ErrMessage = ValidationMessages.InvalidUserRegister;

        return BadRequest(negResp);
      }

      var isUserExists = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

      if (isUserExists != null)
      {
        negResp.Error = ValidationMessages._errorStatuses[400];
        negResp.ErrMessage = ValidationMessages.UserExists;

        return BadRequest(negResp);
      }
      else
      {
        var valid = _userValidationService.IsUserValid(user);

        if (valid.IsValid)
        {
          user.Password = _passwordService.HashPassword(user.Password, null).Value;;
          User newUser = user.ToEntity(_passwordService);

          _context.Users.Add(newUser);
          newUser.CreatedAt = DateTime.Now.ToUniversalTime();
          newUser.UpdatedAt = DateTime.Now.ToUniversalTime();

          await _context.SaveChangesAsync();

          UserRegRespDto createdUser = newUser.ToDto();

          var response = new PosResponse
          {
            Message = ValidationMessages.GenerateSuccessCreationMsg("User"),
            Service = service,
            Data = createdUser
          };

          return Ok(response);
        }
        else
        {
          negResp.Error = ValidationMessages._errorStatuses[400];
          negResp.ErrMessage = string.IsNullOrEmpty(valid.InvalidField)
            ? ValidationMessages.InvalidUserRegister
            : _userValidationService.GetErrByFieldName(valid.InvalidField);
          return BadRequest(negResp);
        }
      }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto user)
    {
      var service = "Login";

      var negResp = new NegResponse
      {
        Error = "",
        ErrMessage = "",
        Service = service,
      };

      if (user == null)
      {
        negResp.Error = ValidationMessages._errorStatuses[400];
        negResp.ErrMessage = ValidationMessages.InvalidUserLogin;

        return BadRequest(negResp);
      }

      var valid = _userValidationService.IsUserLoginValid(user);

      if (valid.IsValid)
      {
       var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

        if (existingUser == null)
        {
          negResp.Error = ValidationMessages._errorStatuses[400];
          negResp.ErrMessage = ValidationMessages.UserNoExists;

          return BadRequest(negResp);
        }
        else
        {
          User foundedUser = existingUser;

          //TODO: check if password valid
          var isPassValid = _passwordService.VerifyPasswordHash(user.Password, foundedUser);

          Console.WriteLine(isPassValid);

          if (isPassValid)
          {
            UserRegRespDto userResp = foundedUser.ToDto();
            var userId = userResp.Id.ToString();
            var username = userResp.Username;
            var accessToken = _jwtTokenService.GenerateToken(userId, username);
            userResp.AccessToken = accessToken;

            HashedDataDto hashedToken = _jwtTokenService.HashToken(accessToken);

            AccessToken token = new()
            {
              Token = hashedToken.Value,
              Salt = hashedToken.Salt,
              CreatedAt = DateTime.Now.ToUniversalTime(),
              UserId = foundedUser.Id
            }; 

            //TODO: add token to db
            var response = new PosResponse
            {
              Service = service,
              Message = ValidationMessages.FoundedEntityMsg("User"),
              Data = userResp
            };

            return Ok(response);
          }
          else
          {
          negResp.Error = ValidationMessages._errorStatuses[400];
          negResp.ErrMessage = ValidationMessages.InvalidCredentials;

          return BadRequest(negResp);
          }
        }
      }
      else
      {
        negResp.Error = ValidationMessages._errorStatuses[401];
        negResp.ErrMessage = ValidationMessages.InvalidCredentials;
        return BadRequest(negResp);
      }
    }
  }
}