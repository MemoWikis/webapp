public class PasswordResetPrepareResult
{
    public bool NoTokenFound;
    public bool TokenOlderThan72h;

    public string Email;
    public bool Success;
}