namespace pet1_backend.Data.Models
{
  public class Role{
    public int Id {get; set;}
    public string Name {get; set;} = string.Empty;
    public ICollection<User> Users {get; set;} = [];
  }
}