using System.IO;
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
        foreach (var file in filesToDelete)
            File.Delete(file);
    }
}