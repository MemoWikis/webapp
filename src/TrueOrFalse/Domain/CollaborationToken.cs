
using System.Security.Cryptography;
using System.Text;

public class CollaborationToken : IRegisterAsInstancePerLifetime
{
    private readonly string _secretKey = Settings.CollaborationTokenSecretKey;

    public string Get(int userId)
    {
        DateTime expiry = DateTime.UtcNow.AddDays(30);
        string dataToSign = $"{userId}:{expiry.ToString("o")}";
        string signature = GenerateSignature(dataToSign);
        string token = $"{userId}:{expiry.ToString("o")}:{signature}";

        return Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
    }

    private string GenerateSignature(string data)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey)))
        {
            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(hashBytes);
        }
    }

    public (bool isValid, int userId) ValidateAndGetUserId(string token)
    {
        int userId;

        string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(token));

        var parts = decodedToken.Split(':');
        if (parts.Length != 3)
        {
            return (false, -1);
        }

        userId = int.Parse(parts[0]);
        string expiryDateStr = parts[1];
        string providedSignature = parts[2];

        if (!DateTime.TryParse(expiryDateStr, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime expiryDate))
        {
            return (false, -1);
        }

        string dataToSign = $"{userId}:{expiryDate.ToString("o")}";
        string expectedSignature = GenerateSignature(dataToSign);

        return (providedSignature == expectedSignature && DateTime.UtcNow <= expiryDate, userId);
    }
}