using Microsoft.AspNetCore.Hosting;

[Serializable]
public class TmpImage
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public TmpImage(int previewWidth, IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
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
        var basePath = _webHostEnvironment.WebRootPath; 
        if (File.Exists(System.IO.Path.Combine(basePath, Path)))
            File.Delete(System.IO.Path.Combine(basePath, Path));

        if (File.Exists(System.IO.Path.Combine(basePath, PathPreview)))
            File.Delete(System.IO.Path.Combine(basePath, PathPreview));
    }

    public Stream GetStream()
    {
        return File.OpenRead(System.IO.Path.Combine(_webHostEnvironment.WebRootPath, Path));
    }

    public Stream RelocateImage(string imgUrl)
    {
        return File.OpenRead(imgUrl);
    }
}

