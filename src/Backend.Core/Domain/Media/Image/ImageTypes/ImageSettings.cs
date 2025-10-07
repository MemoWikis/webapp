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

        Log.Information("ImageSettings.DeleteFiles: Found {FileCount} files to delete for ID {Id} with pattern {Pattern}", 
            filesToDelete.Length, Id, Id + "_*");

        if (filesToDelete.Length > 33)
        {
            Log.Error("ImageSettings.DeleteFiles: Unexpected high amount of files ({FileCount}) for ID {Id}", 
                filesToDelete.Length, Id);
            throw new Exception("unexpected high amount of files");
        }

        foreach (var file in filesToDelete)
        {
            Log.Information("ImageSettings.DeleteFiles: Deleting file {FilePath} for ID {Id}", file, Id);
            File.Delete(file);
        }
    }

    public IImageSettings InitByType(ImageMetaData imageMetaData, QuestionReadingRepo questionReadingRepo)
    {
        switch (imageMetaData.Type)
        {
            case ImageType.Page:
                return new PageImageSettings(imageMetaData.TypeId, _contextAccessor);
            case ImageType.Question:
                return new QuestionImageSettings(imageMetaData.TypeId, _contextAccessor, questionReadingRepo);
            case ImageType.QuestionSet:
                return new SetImageSettings(imageMetaData.TypeId, _contextAccessor);
            case ImageType.User:
                return new UserImageSettings(imageMetaData.TypeId, _contextAccessor);
            case ImageType.PageContent:
                return new PageContentImageSettings(imageMetaData.TypeId, _contextAccessor);
            case ImageType.QuestionContent:
                return new QuestionContentImageSettings(imageMetaData.TypeId, _contextAccessor);
            default:
                throw new Exception("invalid type");
        }
    }
}