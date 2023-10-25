
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public abstract class ImageSettings
{
    protected readonly IHttpContextAccessor _contextAccessor;

    public ImageSettings(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }
    public abstract int Id { get; set;  }
    public abstract string BasePath { get; }

    public string ServerPathAndId()
    {
        return Path.Combine(ImageFolderPath(), BasePath, Id.ToString());
    }

    public static string ImageFolderPath()
    {
        return Path.Combine(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.FullName, "TrueOrFalse.Frontend.Web", "Images");
    }

    public static string SolutionPath()
    {
        return Path.Combine(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.FullName, "TrueOrFalse.Frontend.Web"); 
    }

    public void DeleteFiles()
    {
        var filesToDelete = Directory.GetFiles(ImageFolderPath(), Id + "_*");

        if (filesToDelete.Length > 33)
            throw new Exception("unexpected high amount of files");

        foreach (var file in filesToDelete)
            File.Delete(file);
    }

    public IImageSettings InitByType(ImageMetaData imageMetaData, QuestionReadingRepo questionReadingRepo)
    {
        switch (imageMetaData.Type)
        {
            case ImageType.Category:
                return new CategoryImageSettings(imageMetaData.TypeId, _contextAccessor);
            case ImageType.Question:
                return new QuestionImageSettings(imageMetaData.TypeId, _contextAccessor, questionReadingRepo);
            case ImageType.QuestionSet:
                return new SetImageSettings(imageMetaData.TypeId, _contextAccessor);
            case ImageType.User:
                return new UserImageSettings(imageMetaData.TypeId, _contextAccessor);
            default:
                throw new Exception("invalid type");
        }
    }
}