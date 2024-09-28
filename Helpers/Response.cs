namespace pet1_backend.Helpers
{
  public class PosResponse
  {
    public string Message {get; set;} = string.Empty;
    public string Service {get; set;} = string.Empty;
    public object? Data {get; set;}
  }

  public class NegResponse
  {
    public string Error {get; set;} = string.Empty;
    public string ErrMessage {get; set;} = string.Empty;
    public string Service {get; set;} = string.Empty;
  }
}