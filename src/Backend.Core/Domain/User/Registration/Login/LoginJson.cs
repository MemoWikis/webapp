public class LoginJson
{
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public bool PersistentLogin { get; set; }
}

public readonly record struct LoginRequest(string EmailAddress, string Password, bool PersistentLogin);
