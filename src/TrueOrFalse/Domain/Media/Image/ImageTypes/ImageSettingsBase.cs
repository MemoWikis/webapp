using System.Web;

public abstract class ImageSettingsBase
{
    public abstract int Id { get; set;  }
    public abstract string BasePath { get;  }

    public string ServerPathAndId()
    {
        return HttpContext.Current.Server.MapPath(BasePath + Id);
    }
}