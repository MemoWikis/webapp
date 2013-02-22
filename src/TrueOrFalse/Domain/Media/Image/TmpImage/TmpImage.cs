using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        if (File.Exists(Path)) 
            File.Delete(Path);

        if (File.Exists(PathPreview))
            File.Delete(PathPreview);
    }
}

