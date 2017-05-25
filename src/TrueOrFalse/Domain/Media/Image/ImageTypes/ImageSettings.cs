using System;
using System.IO;
using System.Web;

public abstract class ImageSettings
{
    public abstract int Id { get; set;  }
    public abstract string BasePath { get;  }

    public string ServerPathAndId()
    {
        if (HttpContext.Current == null)
            return "";

        return HttpContext.Current.Server.MapPath(BasePath + Id);
    }

    public string ServerPath()
    {
        if (HttpContext.Current == null)
            return "";

        return HttpContext.Current.Server.MapPath(BasePath);
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