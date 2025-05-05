using static System.String;

public class CredentialsAreValid : IRegisterAsInstancePerLifetime
{
    private readonly UserReadingRepo _userReadingRepo;
    public User User;

    public CredentialsAreValid(UserReadingRepo userReadingRepo)
    {
        _userReadingRepo = userReadingRepo;
    }
    public bool Yes(string emailAdress, string password)
    {
        if(IsNullOrEmpty(emailAdress) || IsNullOrEmpty(password))
            return false;

        User = null;
        var user = _userReadingRepo.GetByEmail(emailAdress.Trim());

        if (user == null)
            return false;

        var isValidPassword = IsValdidPassword.True(password, user);

        if (isValidPassword)
            User = user;

        return isValidPassword;
    }
}