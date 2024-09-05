
public static class ImageDirectoryCreator
{
    public static void CreateImageDirectories(string contentRootPath)
    {
        var imageDirectories = new List<string>
        {
            Settings.TopicImageBasePath,
            Settings.TopicContentImageBasePath,
            Settings.UserImageBasePath,
            Settings.QuestionImageBasePath,
            Settings.QuestionContentImageBasePath,
        };

        foreach (var imageDirectory in imageDirectories)
        {
            CreateImageDirectory(contentRootPath, imageDirectory);
        }
    }

    public static void CreateImageDirectory(string contentRootPath, string path)
    {
        string imagePath = Path.Combine(contentRootPath, "Images", path);

        try
        {
            if (!Directory.Exists(imagePath))
                Directory.CreateDirectory(imagePath);
        }
        catch (Exception ex)
        {
            Logg.r.Error($"Failed to create directory {imagePath}: {ex.Message}");
            throw;
        }
    }
}