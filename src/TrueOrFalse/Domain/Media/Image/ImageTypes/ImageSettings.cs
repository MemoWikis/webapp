
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class ImageSettings
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
    public int Id { get; set;  }
   

    public string ServerPathAndId()
    {
        if (_httpContext == null)
            return "";

        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Id.ToString());
    }

    public string ServerPath()
    {
        if (_httpContext == null)
            return "";

        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
    }

    public void DeleteFiles()
    {
        var filesToDelete = Directory.GetFiles(ServerPath(), Id + "_*");

        if (filesToDelete.Length > 33)
            throw new Exception("unexpected high amount of files");

        foreach (var file in filesToDelete)
            File.Delete(file);
    }

    public IImageSettings InitByType(ImageMetaData imageMetaData)
    {
        switch (imageMetaData.Type)
        {
            case ImageType.Category:
                return new CategoryImageSettings(imageMetaData.TypeId, _contextAccessor, _webHostEnvironment);
            case ImageType.Question:
                return new QuestionImageSettings(imageMetaData.TypeId, _contextAccessor, _webHostEnvironment);
            case ImageType.QuestionSet:
                return new SetImageSettings(imageMetaData.TypeId, _contextAccessor, _webHostEnvironment);
            case ImageType.User:
                return new UserImageSettings(imageMetaData.TypeId, _contextAccessor, _webHostEnvironment);
            default:
                throw new Exception("invalid type");
        }
    }
}