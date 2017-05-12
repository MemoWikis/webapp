using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

public class ImageUrl
{
    public bool HasUploadedImage;
    public string Url;

    public string UrlWithoutTime()
    {
        return Url.Split('?').First();
    }

    public static ImageUrl Get(
        IImageSettings imageSettings,
        int requestedWidth,
        bool isSquare,
        Func<int, string> getFallBackImage)
    {
        var requestedImagePath = imageSettings.ServerPathAndId() + "_" + requestedWidth + SquareSuffix(isSquare) + ".jpg";

        if (imageSettings.Id != -1)
        {
            if (File.Exists(requestedImagePath))
                return GetResult(imageSettings, requestedWidth, isSquare);

            //we guess the biggest file has a width of 512
            var image512 = $"{imageSettings.ServerPathAndId()}_512.jpg";
            if (File.Exists(image512))
            {
                using (var image = Image.FromFile(image512))
                {
                    ResizeImage.Run(image, imageSettings.ServerPathAndId(), requestedWidth, isSquare);
                }
                return GetResult(imageSettings, requestedWidth, isSquare);
            }

            if (HttpContext.Current == null)
            {
                return new ImageUrl {Url = getFallBackImage(requestedWidth), HasUploadedImage = false};
            }

            //we search for the biggest file
            var fileNames = Directory.GetFiles(HttpContext.Current.Server.MapPath(imageSettings.BasePath), $"{imageSettings.Id}_*.jpg");
            if (fileNames.Any()){
                var maxFileWidth = fileNames.Where(x => !x.Contains("s.jpg")).Select(x => Convert.ToInt32(x.Split('_').Last().Replace(".jpg", ""))).OrderByDescending(x => x).First();

                using (var biggestAvailableImage = Image.FromFile($"{imageSettings.ServerPathAndId()}_{maxFileWidth}.jpg"))
                {
                    if (biggestAvailableImage.Width < requestedWidth)//if requested width is bigger than max. available width
                    {
                        if (isSquare)
                        {
                            requestedWidth = Math.Max(biggestAvailableImage.Width, biggestAvailableImage.Height);

                            if (File.Exists(imageSettings.ServerPathAndId() + "_" + requestedWidth + SquareSuffix(true) + ".jpg"))
                                return GetResult(imageSettings, requestedWidth, true);

                            ResizeImage.Run(biggestAvailableImage, imageSettings.ServerPathAndId(), requestedWidth, true);

                        }
                        else
                        {
                            requestedWidth = biggestAvailableImage.Width;
                        }
                    }
                    else
                    {
                        ResizeImage.Run(biggestAvailableImage, imageSettings.ServerPathAndId(), requestedWidth, isSquare);
                    }
                }
                return GetResult(imageSettings, requestedWidth, isSquare);
            }
        }

        return new ImageUrl { Url = getFallBackImage(requestedWidth), HasUploadedImage = false};
    }

    private static ImageUrl GetResult(IImageSettings imageSettings, int width, bool isSquare)
    {
        var url = width ==  -1 ?
                            imageSettings.BasePath + imageSettings.Id + ".jpg" :
                            imageSettings.BasePath + imageSettings.Id + "_" + width + SquareSuffix(isSquare) + ".jpg";
        
        return new ImageUrl
        {
            Url = url + "?t=" + File.GetLastWriteTime(HttpContext.Current.Server.MapPath(url)).ToString("yyyyMMddhhmmss"),
            HasUploadedImage = true
        };
    }

    public static string SquareSuffix(bool isSquare){
        return isSquare ?  "s" : "";
    }

    public ImageUrl SetSuffix(ImageMetaData imageMeta)
    {
        var urlSuffix = "";
        if (imageMeta != null && !imageMeta.IsYoutubePreviewImage)
            urlSuffix = "?" + imageMeta.DateModified.ToString("yyyyMMdd-HHMMss");

        Url += urlSuffix ;
        return this;
    }

    public static string GetFallbackImageUrl(IImageSettings imageSettings, int width)
    {
        if (File.Exists(HttpContext.Current.Server.MapPath(imageSettings.BaseDummyUrl) + width + ".png"))
            return imageSettings.BaseDummyUrl + width + ".png";

        //Get next bigger image or maximum size image
        var fileNameTrunk = imageSettings.BaseDummyUrl.Split('/').Last();
        var fileDirectory = imageSettings.BaseDummyUrl.Split(new string[] {fileNameTrunk}, StringSplitOptions.None).First();
        var fileNames = Directory.GetFiles(HttpContext.Current.Server.MapPath(fileDirectory), fileNameTrunk + "*.png");
        if (fileNames.Any())
        {
            var fileWidths = fileNames.Where(x => !x.Contains("s.jpg")).Select(x => Convert.ToInt32(x.Split('-').Last().Replace(".png", ""))).OrderByDescending(x => x).ToList();
            var fileWidth = 0;
            for (var i = 0; i < fileWidths.Count(); i++)
            {
                if (fileWidths[i] < width) {
                    fileWidth = i == 0 ? fileWidths[0] : fileWidths[i - 1];
                    break;
                }
            }
            if (fileWidth == 0)
            {
                fileWidth = fileWidths.Last();
            }
            if (File.Exists(HttpContext.Current.Server.MapPath(imageSettings.BaseDummyUrl) + fileWidth + ".png"))
            {
                return imageSettings.BaseDummyUrl + fileWidth + ".png";
            }
        }
        return "";
    }
}