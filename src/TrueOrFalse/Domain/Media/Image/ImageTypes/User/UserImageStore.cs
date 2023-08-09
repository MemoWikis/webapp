using System.Web;
using Microsoft.AspNetCore.Http;

public class UserImageStore
{
    public static void Run(IFormFile imageFile, int userId)
    {
        if (imageFile == null || imageFile.Length == 0)
            return;

        using var stream = imageFile.OpenReadStream();

        SaveImageToFile.Run(
            stream,
            new UserImageSettings(userId)
        );
    }
    






}