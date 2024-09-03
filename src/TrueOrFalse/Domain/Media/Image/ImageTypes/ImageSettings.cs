
using Microsoft.AspNetCore.Http;

public abstract class ImageSettings
{
    protected readonly IHttpContextAccessor _contextAccessor;

    public ImageSettings(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }
    public abstract int Id { get; set; }
    public abstract string BasePath { get; }

    public string ServerPathAndId()
    {
        return Path.Combine(Settings.ImagePath, BasePath, Id.ToString());
    }

    public void DeleteFiles()
    {
        var filesToDelete = Directory.GetFiles(Settings.ImagePath, Id + "_*");

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
            case ImageType.TopicContent:
                return new TopicContentImageSettings(imageMetaData.TypeId, _contextAccessor);
            default:
                throw new Exception("invalid type");
        }
    }
}