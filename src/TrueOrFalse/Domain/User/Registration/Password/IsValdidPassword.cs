public class IsValdidPassword : IRegisterAsInstancePerLifetime
{
    public bool True(string givenPasswordString, User user)
    {
        return True(givenPasswordString, user.PasswordHashedAndSalted, user.Salt);
    }

    private bool True(string givenPasswordString, string dbPassword, string dbSalt)
    {
        return dbPassword == HashPassword.Run(givenPasswordString.Trim(), dbSalt);
    }
}