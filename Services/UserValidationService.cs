using System.Text.RegularExpressions;
using pet1_backend.Dtos.Account;
using pet1_backend.Validation;

namespace pet1_backend.Services
{
    public interface IUserValidationService
    {
      bool IsValidUsername(string username);
      bool IsValidEmail(string email);
      bool IsValidPassword(string password);
      UserDtoError IsUserValid(UserRegDto user);
      UserDtoError IsUserLoginValid(UserLoginDto user);
      string GetErrByFieldName(string fieldName);
    }

    public enum UserFields {
      Username,
      Email,
      Password
    }

    public class UserDtoError
    {
      public bool IsValid {get; set;}
      public string? InvalidField {get; set;}
    }

    public class UserValidationService : IUserValidationService
    {
      const string UsernameReg = "^[A-Za-z0-9]{3,16}$";
      const string EmailReg = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
      const string PasswordReg = @"^(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{8,20}$";
      public bool IsValidUsername(string username)
      {
          if (string.IsNullOrEmpty(username))
            return false;

          return Regex.IsMatch(username, UsernameReg);
      }
      public bool IsValidEmail(string email)
      {
          if (string.IsNullOrEmpty(email))
            return false;

          return Regex.IsMatch(email, EmailReg);
      }

      public bool IsValidPassword(string password)
      {
        if (string.IsNullOrEmpty(password))
          return false;

        return Regex.IsMatch(password, PasswordReg);
      }

      public UserDtoError IsUserValid(UserRegDto user)
      {
        var result = new UserDtoError { IsValid = false };
        UserFields[] fields = [UserFields.Username, UserFields.Email, UserFields.Password];

        foreach (var field in fields)
        {
          if (!IsValidField(user, field))
          {
            result.InvalidField = field.ToString();
            return result;
          }
        }

        result.IsValid = true;
        return result;
      }

      public UserDtoError IsUserLoginValid(UserLoginDto user)
      {
        var result = new UserDtoError { IsValid = false };
        UserFields[] fields = [UserFields.Email, UserFields.Password];

        foreach (var field in fields)
        {
          if (!IsValidLoginField(user, field))
          {
            result.InvalidField = field.ToString();
            return result;
          }
        }

        result.IsValid = true;
        return result;
      }

      private bool IsValidField(UserRegDto user, UserFields field)
      {
        switch(field)
        {
          case UserFields.Username:
            return IsValidUsername(user.Username);
          case UserFields.Email:
            return IsValidEmail(user.Email);
          case UserFields.Password:
            return IsValidPassword(user.Password);
          default:
            return false;
        }
      }

      private bool IsValidLoginField(UserLoginDto user, UserFields field)
      {
        switch(field)
        {
          case UserFields.Email:
            return IsValidEmail(user.Email);
          case UserFields.Password:
            return IsValidPassword(user.Password);
          default:
            return false;
        }
      }

      public string GetErrByFieldName(string fieldName)
      {
        UserFields testName = (UserFields)Enum.Parse(typeof(UserFields),fieldName);
        switch(testName)
        {
          case UserFields.Username:
            return ValidationMessages.InvalidUserName;
          case UserFields.Email:
            return ValidationMessages.InvalidEmail;
          case UserFields.Password:
            return ValidationMessages.InvalidPassword;
          default:
            return ValidationMessages.InvalidUserRegister;
        }
      }
    }
}