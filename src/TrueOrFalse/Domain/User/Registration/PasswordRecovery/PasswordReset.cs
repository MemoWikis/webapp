public class PasswordReset : IRegisterAsInstancePerLifetime
{
    private readonly PasswordResetPrepare _passwordResetPrepare;
    private readonly UserRepo _userRepo;

    public PasswordReset(PasswordResetPrepare passwordResetPrepare, 
                            UserRepo userRepo)
    {
        _passwordResetPrepare = passwordResetPrepare;
        _userRepo = userRepo;
    }

    public bool Run(string token, string newPassword)
    {
        var passwortResetPrepareResult = _passwordResetPrepare.Run(token);
        if (!passwortResetPrepareResult.Success)
            return false;

        var user = _userRepo.GetByEmail(passwortResetPrepareResult.Email);
        SetUserPassword.Run(newPassword, user);
        _userRepo.Update(user);

        return true;
    }
}