using System;

public class ValidateEmailConfirmationKey
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
        if (!Int32.TryParse(affirmationKey.Substring(3), out userId) == false)
            return false;

        if (_userRepo.GetById(userId) != null)
            return true;

        return true;
    }
}