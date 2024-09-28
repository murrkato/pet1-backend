using System.ComponentModel.DataAnnotations;

namespace pet1_backend.Dtos.Account
{
  public class UserRegDto
  {
    // [Required]
    // [RegularExpression("^[A-Za-z0-9]{3,16}$")]
    public string Username {get; set;} = string.Empty;
    // [Required]
    // [EmailAddress]
    public string Email {get; set;} = string.Empty;
    // [Required]
    // [RegularExpression("^(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{8,20}$")]
    public string Password {get; set;} = string.Empty;
  }
}