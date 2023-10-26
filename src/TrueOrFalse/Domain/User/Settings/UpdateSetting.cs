using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class UpdateSetting
{
    public static string HashUpdateCommand(User user,
        string updateCommand, 
        HttpContext httpContext, 
        IWebHostEnvironment webHostEnvironment)
    {
        var salt = user.Salt;
        MD5 md5Hasher = MD5.Create();
        byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(updateCommand + salt + Settings.UpdateUserSettingsKey));

        var stringBuilder = new StringBuilder();

        for (int i = 0; i < data.Length; i++)
            stringBuilder.Append(data[i].ToString("x2"));


        return stringBuilder.ToString();
    }

    public static bool IsValidUpdateCommand(User user,
        string updateCommand, 
        string token, 
        HttpContext httpContext, 
        IWebHostEnvironment webHostEnvironment)
    {
        if (user == null)
            return false;

        return token == HashUpdateCommand(user, updateCommand, httpContext, webHostEnvironment);
    }
}
