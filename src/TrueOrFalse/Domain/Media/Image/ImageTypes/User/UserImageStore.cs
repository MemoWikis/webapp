using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class UserImageStore
{
    public static void Run(IFormFile imageFile, int userId, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
    {
        if (imageFile == null || imageFile.Length == 0)
            return;

        using var stream = imageFile.OpenReadStream();
        var userImageSettings = new UserImageSettings(userId, httpContextAccessor, webHostEnvironment); 
        SaveImageToFile.Run(
        stream,
        userImageSettings,
        webHostEnvironment,
        httpContextAccessor);
    }







}