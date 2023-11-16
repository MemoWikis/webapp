using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class UpdateUserImage
{
    public static void Run(IFormFile imageFile, int userId, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment, Logg logg)
    {
        if (imageFile == null || imageFile.Length == 0)
            return;

        using var stream = imageFile.OpenReadStream();
        var userImageSettings = new UserImageSettings(userId, httpContextAccessor); 
        SaveImageToFile.RemoveExistingAndSaveAllSizes(
            stream,
            userImageSettings
        );
    }
}