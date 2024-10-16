namespace pet1_backend.Dtos.Account.Client
{
  public class HashedDataDto {
    public string Value {get; set;} = string.Empty;
    public byte[] Salt {get; set;} = [];
    public DateTime? Expires {get; set;}
  }
}