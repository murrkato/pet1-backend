namespace pet1_backend.Validation
{
  public class ValidationMessages
  {
    public const string InvalidUserRegister = "Username, email and password are required";
    public const string RequiredFieldTemplate = "{0} is required";
    public const string InvalidUserName = "Username should be 3-16 characters and shouldn't include any special character";
    public const string InvalidEmail = "Invalid email format";
    public const string InvalidPassword = "Password should be 8-20 characters and include at least 1 letter, 1 number and 1 special character";
    public const string InvalidUserLogin = "Credencials are required";
    
    //resp errors
    public static Dictionary<int, string> _errorStatuses = new() {
      {400, "Failed"},
      {401, "Unauthorized"},
      {404, "Not found"}
    };
    public const string UserExists = "User with such email is already exists";
    public const string UserNoExists = "There is no user with such email";
    public const string successCreation = "{0} is succesfully created";
    public const string InvalidCredentials = "Incorrect email or password";
    public static string GenerateRequiredMsg(string fieldName)
    {
      return string.Format(RequiredFieldTemplate, fieldName);
    }
    public static string GenerateSuccessCreationMsg(string fieldName)
    {
      return string.Format(successCreation, fieldName);
    }
    public static string FoundedEntityMsg(string entityName, string type = "found")
    {
       if (type == "found")
       {
        return $"{entityName} found.";
       }
       else
       {
        return $"{entityName} not found.";
       }
    }
  }
}