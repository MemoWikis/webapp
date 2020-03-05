using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

[Serializable]
public class TmpImage
{
    public TmpImage(int previewWidth)
    {
        string deleteTime = DateTime.Now.AddHours(1).ToString("yyyy-mm-dd_hh-MM");
        Guid = System.Guid.NewGuid().ToString();
        Path = "/Images/Tmp/" + Guid + "-DEL-" + deleteTime + ".png";
        PathPreview = "/Images/Tmp/" + Guid + "-" + previewWidth + "-DEL-" + deleteTime + ".jpg";
        PreviewWidth = previewWidth;
    }

    public string Guid { get; private set; }
    public string Path { get; private set; }
    public string PathPreview { get; private set; }

    public int PreviewWidth { get; private set; }

    ~TmpImage()
    {
        if (HttpContext.Current == null)
            return;

        if (File.Exists(HttpContext.Current.Server.MapPath(Path)))
            File.Delete(HttpContext.Current.Server.MapPath(Path));

        if (File.Exists(HttpContext.Current.Server.MapPath(PathPreview)))
            File.Delete(HttpContext.Current.Server.MapPath(PathPreview));
    }

    public Stream GetStream()
    {
        return File.OpenRead(HttpContext.Current.Server.MapPath(Path));
    }

    public Stream RelocateImage(string imgUrl)
    {
        return File.OpenRead(imgUrl);
    }
}

