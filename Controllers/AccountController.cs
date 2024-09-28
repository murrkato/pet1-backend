using Microsoft.AspNetCore.Mvc;
using pet1_backend.Dtos.Account;
using pet1_backend.Helpers;
using pet1_backend.Services;
using pet1_backend.Validation;

namespace pet1_backend.Controllers
{
  [ApiController]
  public class AccountController : ControllerBase
  {
    private readonly IUserValidationService _userValidationService;

    public AccountController (IUserValidationService userValidationService)
    {
      _userValidationService = userValidationService;
    }
    [HttpPost("register")]
    public IActionResult Authenticate([FromBody] UserRegDto user)
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
        negResp.ErrMessage = ValidationMessages.UserExists;

        return BadRequest(negResp);
      }

      //TODO:if user exists

      var isUserExists = false;

      if (isUserExists)
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
          //TODO: replace with UserRegRespDto
          var newUser = new 
          {
              Username = user.Username,
              Token = "Token"
          };

          var response = new PosResponse
          {
            Service = service,
            Data = newUser
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
  }
}