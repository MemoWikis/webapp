public class ValidateEmailConfirmationKey : IRegisterAsInstancePerLifetime
{
    private readonly UserRepo _userRepo;

    public ValidateEmailConfirmationKey(UserRepo userRepo)
    {
        _userRepo = userRepo;
    }

    public bool IsValid(string affirmationKey)
    {
        if (affirmationKey.Length <= 4)
            return false;

        int userId;
        if (!Int32.TryParse(affirmationKey.Substring(3), out userId))
            return false;

        var user = _userRepo.GetById(userId);
        if (user != null)
        {
            user.IsEmailConfirmed = true;
            _userRepo.Update(user);
            return true;
        }

        return true;
    }
}