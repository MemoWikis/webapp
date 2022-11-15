using static System.String;

public class CredentialsAreValid : IRegisterAsInstancePerLifetime
{
    public User User;

    public bool Yes(string emailAdress, string password)
    {
        if(IsNullOrEmpty(emailAdress) || IsNullOrEmpty(password))
            return false;

        User = null;
        var user = Sl.R<UserRepo>().GetByEmailEager(emailAdress.Trim());

        if (user == null)
            return false;

        var isValidPassword = IsValdidPassword.True(password, user);

        if (isValidPassword)
            User = user;

        return isValidPassword;
    }
}