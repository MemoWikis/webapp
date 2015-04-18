public class PasswordReset : IRegisterAsInstancePerLifetime
{
    private readonly PasswordResetPrepare _passwordResetPrepare;
    private readonly UserRepository _userRepository;

    public PasswordReset(PasswordResetPrepare passwordResetPrepare, 
                            UserRepository userRepository)
    {
        _passwordResetPrepare = passwordResetPrepare;
        _userRepository = userRepository;
    }

    public bool Run(string token, string newPassword)
    {
        var passwortResetPrepareResult = _passwordResetPrepare.Run(token);
        if (!passwortResetPrepareResult.Success)
            return false;

        var user = _userRepository.GetByEmail(passwortResetPrepareResult.Email);
        SetUserPassword.Run(newPassword, user);
        _userRepository.Update(user);

        return true;
    }
}