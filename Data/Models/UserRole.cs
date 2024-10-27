using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pet1_backend.Data.Models
{
  public class UserRole
  {
    [ForeignKey("User")]
    public Guid UserId {get; set;}
    public virtual User User {get; set;} = null!;
    [ForeignKey("Role")]
    public int RoleId {get; set;}
    public virtual Role Role {get; set;} = null!;
  }
}