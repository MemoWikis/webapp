using System.Globalization;
using System.Security.Cryptography;
using System.Text;

public class EmailConfirmationService: IRegisterAsInstancePerLifetime
{
    private readonly UserRepo _userRepo;

    public EmailConfirmationService(UserRepo userRepo)
    {
        _userRepo = userRepo;
    }

    public static string CreateEmailConfirmationToken(User user)
    {
        DateTime formattedDateTime = DateTime.Parse(user.DateCreated.ToString("yyyy-MM-dd HH:mm:ss"));
        long dateTimeInMilliseconds = ((DateTimeOffset)formattedDateTime).ToUnixTimeMilliseconds();
        string rawString = dateTimeInMilliseconds + user.PasswordHashedAndSalted.Substring(0, Math.Min(user.PasswordHashedAndSalted.Length, 3)) + "|" + user.EmailAddress;

        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawString));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            var token = builder + "-" + user.Id;
            return token;
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