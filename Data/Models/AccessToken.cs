using System.ComponentModel.DataAnnotations.Schema;

namespace pet1_backend.Data.Models
{
  public class AccessToken
  {
    public Guid Id {get; set;}
    public string Token {get; set;} = string.Empty;
    public byte[] Salt {get; set;} = [];
    public DateTime CreatedAt {get; set;}
    public DateTime ExpiresAt {get; set;}
    [ForeignKey("User")]
    public Guid UserId {get; set;}
    public virtual User? User {get; set;}
  }
}