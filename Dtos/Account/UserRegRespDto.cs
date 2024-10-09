using System.ComponentModel.DataAnnotations;

namespace pet1_backend.Dtos.Account
{
  public class UserRegRespDto
  {
    [Required]
    public Guid Id {get; set;}
    [Required]
    public string Username {get; set;} = string.Empty;
    [Required]
    public string Email {get; set;} = string.Empty;
    public string? AccessToken {get; set;}
  }
}