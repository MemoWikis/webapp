using System;

public class CredentialsAreValid : IRegisterAsInstancePerLifetime
{
    private readonly UserRepo _userRepo;
    private readonly IsValdidPassword _isValidPassword;
        
    public User User;

    public CredentialsAreValid(UserRepo userRepo, 
                                IsValdidPassword isValidPassword)
    {
        _userRepo = userRepo;
        _isValidPassword = isValidPassword;
    }

    public bool Yes(string emailAdress, string password)
    {
        if(String.IsNullOrEmpty(emailAdress) || String.IsNullOrEmpty(password))
            return false;

        User = null;
        var user = _userRepo.GetByEmail(emailAdress.Trim());

        if (user == null)
            return false;

        var isValidPassword =  _isValidPassword.True(password, user);

        if (isValidPassword)
            User = user;

        return isValidPassword;
    }
}