using static System.String;

public class CredentialsAreValid : IRegisterAsInstancePerLifetime
{
    private readonly UserRepo _userRepo;
    public User User;

    public CredentialsAreValid(UserRepo userRepo)
    {
        _userRepo = userRepo;
    }
    public bool Yes(string emailAdress, string password)
    {
        if(IsNullOrEmpty(emailAdress) || IsNullOrEmpty(password))
            return false;

        User = null;
        var user = _userRepo.GetByEmailEager(emailAdress.Trim());

        if (user == null)
            return false;

        var isValidPassword = IsValdidPassword.True(password, user);

        if (isValidPassword)
            User = user;

        return isValidPassword;
    }
}