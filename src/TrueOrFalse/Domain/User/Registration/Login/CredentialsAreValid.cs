using System;

public class CredentialsAreValid : IRegisterAsInstancePerLifetime
{
    private readonly UserRepository _userRepository;
    private readonly IsValdidPassword _isValidPassword;
        
    public User User;

    public CredentialsAreValid(UserRepository userRepository, 
                                IsValdidPassword isValidPassword)
    {
        _userRepository = userRepository;
        _isValidPassword = isValidPassword;
    }

    public bool Yes(string emailAdress, string password)
    {
        if(String.IsNullOrEmpty(emailAdress) || String.IsNullOrEmpty(password))
            return false;

        User = null;
        var user = _userRepository.GetByEmail(emailAdress.Trim());

        if (user == null)
            return false;

        var isValidPassword =  _isValidPassword.True(password, user);

        if (isValidPassword)
            User = user;

        return isValidPassword;
    }
}