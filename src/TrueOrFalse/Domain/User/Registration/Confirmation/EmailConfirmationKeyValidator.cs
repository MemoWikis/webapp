public class EmailConfirmationService: IRegisterAsInstancePerLifetime
{
    private readonly UserRepo _userRepo;

    public EmailConfirmationService(UserRepo userRepo)
    {
        _userRepo = userRepo;
    }

    public bool ConfirmUserEmailByKey(string affirmationKey)
    {
        if (string.IsNullOrEmpty(affirmationKey) || affirmationKey.Length <= 4)
        {
            return false;
        }

        if (!TryParseUserIdFromKey(affirmationKey, out int userId))
        {
            return false;
        }

        return ConfirmAndSaveUserEmail(userId);
    }

    private bool TryParseUserIdFromKey(string affirmationKey, out int userId)
    {
        return int.TryParse(affirmationKey.Substring(3), out userId);
    }

    private bool ConfirmAndSaveUserEmail(int userId)
    {
        var user = _userRepo.GetById(userId);
        if (user == null)
        {
            return false;
        }

        user.IsEmailConfirmed = true;
        _userRepo.Update(user);

        return true;
    }

}