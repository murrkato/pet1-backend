namespace pet1_backend.Dtos.Account.Client
{
  public class HashedDataDto {
    public string Value {get; set;} = string.Empty;
    public byte[] Salt {get; set;} = [];
    public string? Expires {get; set;} = string.Empty;
  }
}