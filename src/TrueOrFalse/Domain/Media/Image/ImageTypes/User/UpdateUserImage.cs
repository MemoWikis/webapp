using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class UpdateUserImage
{
    public static void Run(IFormFile imageFile, int userId, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment, Logg logg)
    {
        if (imageFile == null || imageFile.Length == 0)
            return;

        DeleteExistingUserImages(userId, webHostEnvironment);

        using var stream = imageFile.OpenReadStream();
        var userImageSettings = new UserImageSettings(userId, httpContextAccessor); 
        SaveImageToFile.Run(
            stream,
            userImageSettings,
            logg
        );
    }

    private static void DeleteExistingUserImages(int userId, IWebHostEnvironment webHostEnvironment)
    {
        var directory = Path.Combine(Settings.ImagePath, "Users");

        var userImagePattern = $"{userId}_*";
        var files = Directory.GetFiles(directory, userImagePattern);

        foreach (var file in files)
        {
            File.Delete(file);
        }
    }
}