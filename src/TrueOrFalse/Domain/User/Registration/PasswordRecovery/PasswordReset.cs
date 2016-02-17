public class PasswordReset : IRegisterAsInstancePerLifetime
{
    private readonly UserRepo _userRepo;

    public PasswordReset(UserRepo userRepo){
        _userRepo = userRepo;
    }

    public bool Run(string token, string newPassword)
    {
        var passwortResetPrepareResult = PasswordResetPrepare.Run(token);
        if (!passwortResetPrepareResult.Success)
            return false;

        var user = _userRepo.GetByEmail(passwortResetPrepareResult.Email);
        SetUserPassword.Run(newPassword, user);
        _userRepo.Update(user);

        return true;
    }
}