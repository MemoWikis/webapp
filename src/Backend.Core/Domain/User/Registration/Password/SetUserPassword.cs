public class SetUserPassword
{
    public static void Run(string password, User user)
    {
        user.Salt = Guid.NewGuid().ToString();
        user.PasswordHashedAndSalted = HashPassword.Run(password, user.Salt);
    }
}