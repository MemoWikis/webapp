using System.IO;
using System.Linq;
using System.Web;
using Seedworks.Lib.Persistence;

public abstract class GetImageUrl<T> where T: IPersistable
{
    protected abstract string RelativePath { get; }

    public GetImageUrlResult Run(T entity)
    {
        var serverPath = HttpContext.Current.Server.MapPath(RelativePath);
        if (Directory.GetFiles(serverPath, string.Format("{0}_*.jpg", entity.Id)).Any())
            return new GetImageUrlResult{Url = RelativePath + entity.Id + "_{0}.jpg", HasUploadedImage = true};

        return new GetImageUrlResult {Url = GetFallbackImage(entity), HasUploadedImage = false};
    }

    protected abstract string GetFallbackImage(T entity);

}