public class LoginJson
{
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public bool PersistentLogin { get; set; }
}

public readonly record struct LoginParam(string EmailAddress, string Password, bool PersistentLogin);
