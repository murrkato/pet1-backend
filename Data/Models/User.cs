namespace pet1_backend.Data.Models
{
  public class User
  {
    public Guid Id {get; set;}
    public string Username {get; set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
    // [ForeignKey("Role")]
    // public Guid RoleId {get; set;}
    // public virtual Role? Role {get; set;}
    public string PasswordHash {get;  set;} = string.Empty;
    public byte[] Salt {get;  set;} = [];
    public string AccessToken {get; set;} = string.Empty;
    public DateTime CreatedAt {get; set;}
    public DateTime UpdatedAt {get; set;}
  }
}