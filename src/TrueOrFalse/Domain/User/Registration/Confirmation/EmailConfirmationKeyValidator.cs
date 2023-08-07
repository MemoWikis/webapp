using System.Security.Cryptography;
using System.Text;

public class EmailConfirmationService: IRegisterAsInstancePerLifetime
{
    private readonly UserRepo _userRepo;

    public EmailConfirmationService(UserRepo userRepo)
    {
        _userRepo = userRepo;
    }

    //public bool ConfirmUserEmailByKey(string affirmationKey)
    //{
    //    if (string.IsNullOrEmpty(affirmationKey) || affirmationKey.Length <= 4)
    //    {
    //        return false;
    //    }

    //    if (!TryParseUserIdFromKey(affirmationKey, out int userId))
    //    {
    //        return false;
    //    }

    //    return ConfirmAndSaveUserEmail(userId);
    //}

    //private bool TryParseUserIdFromKey(string affirmationKey, out int userId)
    //{
    //    return int.TryParse(affirmationKey.Substring(3), out userId);
    //}

    //private bool ConfirmAndSaveUserEmail(int userId)
    //{
    //    var user = _userRepo.GetById(userId);
    //    if (user == null)
    //    {
    //        return false;
    //    }

    //    user.IsEmailConfirmed = true;
    //    _userRepo.Update(user);

    //    return true;
    //}

    public static string CreateEmailConfirmationToken(User user)
    {
        long dateTimeInMilliseconds = ((DateTimeOffset)user.DateCreated).ToUnixTimeMilliseconds();
        string rawString = dateTimeInMilliseconds + user.PasswordHashedAndSalted;

        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawString));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder + "-" + user.Id;
        }
    }

    public bool TryConfirmEmail(string token)
    {
        int userId = ExtractUserIdFromToken(token);
        User user = _userRepo.GetById(userId);
        if (user != null || userId < 1)
        {
            string recreatedToken = CreateEmailConfirmationToken(user);
            return recreatedToken == token;
        }

        return false;
    }

    private int ExtractUserIdFromToken(string token)
    {
        string[] parts = token.Split('-');
        string userIdString = parts[1];
        if (!int.TryParse(userIdString, out int userId))
        {
            return -1;
        }
        return userId;
    }

    public bool TryConfirmEmailTest(string token, User user)
    {
        int userId = ExtractUserIdFromToken(token);
        if (user != null || userId != user.Id || userId < 1)
        {
            string recreatedToken = CreateEmailConfirmationToken(user);
            return recreatedToken == token;
        }

        return false;
    }
}