
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public abstract class ImageSettings
{
    protected readonly IHttpContextAccessor _contextAccessor;
    protected readonly IWebHostEnvironment _webHostEnvironment;
    private readonly HttpContext? _httpContext;

    public ImageSettings(IHttpContextAccessor contextAccessor, IWebHostEnvironment webHostEnvironment)
    {
        _contextAccessor = contextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _httpContext = _contextAccessor.HttpContext;
    }
    public abstract int Id { get; set;  }
    public abstract string BasePath { get;  }

    public string ServerPathAndId()
    {
        if (_httpContext == null)
            return "";

        return Path.Combine(_webHostEnvironment.WebRootPath, BasePath, Id.ToString());
    }

    public string ServerPath()
    {
        if (_httpContext == null)
            return "";

        return Path.Combine(_webHostEnvironment.WebRootPath, BasePath);
    }

    public void DeleteFiles()
    {
        var filesToDelete = Directory.GetFiles(ServerPath(), Id + "_*");

        if (filesToDelete.Length > 33)
            throw new Exception("unexpected high amount of files");

        foreach (var file in filesToDelete)
            File.Delete(file);
    }

    public static IImageSettings InitByType(ImageMetaData imageMetaData)
    {
        switch (imageMetaData.Type)
        {
            case ImageType.Category:
                return new CategoryImageSettings(imageMetaData.TypeId);
            case ImageType.Question:
                return new QuestionImageSettings(imageMetaData.TypeId);
            case ImageType.QuestionSet:
                return new SetImageSettings(imageMetaData.TypeId);
            case ImageType.User:
                return new UserImageSettings(imageMetaData.TypeId);
            default:
                throw new Exception("invalid type");
        }
    }
}