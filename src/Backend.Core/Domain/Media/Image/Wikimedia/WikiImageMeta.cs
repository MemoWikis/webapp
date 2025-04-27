using System.Net;

public class WikiImageMeta
{
    public string ApiHost;

    public bool ImageNotFound;

    public int PageId;
    public int PageNamespace;
    public string ImageTitle;
    public string ImageRepository;

    public DateTime ImageTimeStamp;

    /// <summary>Original Image Width</summary>
    public int ImageOriginalWidth;

    /// <summary>Original Image Height</summary>
    public int ImageOriginalHeight;

    /// <summary>Original Image URL</summary>
    public string ImageOriginalUrl;

    public string ImageUrlDescription;

    public int ImageWidth;
    public int ImageHeight;
    public string ImageUrl;

    public string User;
    public string UserId;

    public string JSonResult;

    public Stream GetThumbImageStream()
    {
        var request = (HttpWebRequest)WebRequest.Create(ImageUrl);
        WikiImageMetaLoader.SetUserAgent(request);
        var response = (HttpWebResponse)request.GetResponse();

        return response.GetResponseStream();
    }
}