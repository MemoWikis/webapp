using System;
using System.IO;
using System.Linq;
using System.Web;

public abstract class ImageSettingsBase
{
    public abstract int Id { get; set;  }
    public abstract string BasePath { get;  }

    public string ServerPathAndId()
    {
        return HttpContext.Current.Server.MapPath(BasePath + Id);
    }

    public string ServerPath()
    {
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
}