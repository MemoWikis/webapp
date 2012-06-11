using System.IO;
using System.Linq;
using System.Web;

public abstract class GetImageUrl
{
    protected abstract string PlaceholderImage { get; }
    protected abstract string RelativePath { get; }

    protected string Run(int id)
    {
        var serverPath = HttpContext.Current.Server.MapPath(RelativePath);
        if (Directory.GetFiles(serverPath, string.Format("{0}_*.jpg", id)).Any())
        {
            return RelativePath + id + "_{0}.jpg";
        }
        else
        {
            return PlaceholderImage;
        }
    }
}