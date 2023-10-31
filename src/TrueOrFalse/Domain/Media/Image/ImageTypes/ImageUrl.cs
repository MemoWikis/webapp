using System.Drawing;
using Microsoft.AspNetCore.Http;


public class ImageUrl
{
    public bool HasUploadedImage;
    public string Url;
    private readonly HttpContext? _httpContext;
    private static string ImagePath => Settings.ImagePath;
    private static string ImageFolder = "/Images";

    public ImageUrl(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext;
    }

    public string UrlWithoutTime()
    {
        return Url.Split('?').First();
    } 

    public ImageUrl Get(
        IImageSettings imageSettings,
        int requestedWidth,
        bool isSquare,
        Func<int, string> getFallBackImage)
    {
        var requestedImagePath =  Path.Combine(ImagePath, imageSettings.BasePath, $"{imageSettings.Id}_" + requestedWidth + SquareSuffix(isSquare) + ".jpg").NormalizePathSeparators();

        if (imageSettings.Id > 0)
        {
            if (File.Exists(requestedImagePath))
                return GetResult(imageSettings, requestedWidth, isSquare);

            //we guess the biggest file has a width of 512
            var image512 = Path.Combine(ImagePath, "_512.jpg");
            if (File.Exists(image512))
            {
                using (var image = Image.FromFile(image512))
                {
                    ResizeImage.Run(image, imageSettings.ServerPathAndId(), requestedWidth, isSquare);
                }
                return GetResult(imageSettings, requestedWidth, isSquare);
            }

            if (_httpContext == null)
            {
                Url = getFallBackImage(requestedWidth);
                HasUploadedImage = false;
                return this;
            }

            //we search for the biggest file
           
            var searchPattern = $"{imageSettings.Id}_*.jpg";
            
            var basePath = Path.Combine(ImagePath, imageSettings.BasePath).NormalizePathSeparators();
            if (Directory.Exists(basePath) == false)
            {
                Logg.r.Error("Directory is not available");
            }

            var fileNames = Directory.GetFiles(basePath, searchPattern);
           
            if (fileNames.Any()){
                var maxFileWidth = fileNames
                    .Where(x => !x.Contains("s.jpg"))
                    .Select(x => Convert.ToInt32(x.Split('_').Last()
                        .Replace(".jpg", "")))
                    .OrderByDescending(x => x)
                    .First();

                using (var biggestAvailableImage = Image.FromFile($"{imageSettings.ServerPathAndId()}_{maxFileWidth}.jpg"))
                {
                    if (biggestAvailableImage.Width < requestedWidth)//if requested width is bigger than max. available width
                    {
                        var absoluteUri = $"{_httpContext.Request.Scheme}://{_httpContext.Request.Host}{_httpContext.Request.Path}{_httpContext.Request.QueryString}";
                        Logg.r.Warning($"Requested image width of {requestedWidth}px is greater than max. available {biggestAvailableImage.Width}px of image {imageSettings.ServerPathAndId()} (requested url: {absoluteUri}). ");

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

        var url = Path.Combine(ImageFolder, getFallBackImage(requestedWidth));
        Url =  url.NormalizePathSeparators();
        Console.WriteLine(url);
        HasUploadedImage = false;
        return this;
    }

    private ImageUrl GetResult(IImageSettings imageSettings, int width, bool isSquare)
    {
        var basePath = $"{imageSettings.BasePath}/{imageSettings.Id}"; 
        var url = width ==  -1 ?
                             basePath + ".jpg" :
                            basePath + "_" + width + SquareSuffix(isSquare) + ".jpg";
        Url = url + "?t=" + File.GetLastWriteTime(Path.Combine(Settings.ImagePath, url))
            .ToString("yyyyMMddhhmmss");
        HasUploadedImage = true;
        
        return this;
    }

    public static string SquareSuffix(bool isSquare){
        return isSquare ?  "s" : "";
    }

    public string GetFallbackImageUrl(IImageSettings imageSettings, int width)
    {
        var filePath =
            Path.Combine(Settings.ImagePath, imageSettings.BaseDummyUrl);
        
        if (File.Exists(filePath + width + ".png"))
            return imageSettings.BaseDummyUrl + width + ".png";

        //Get next bigger image or maximum size image
        return GetNextBiggerOrMaxSizeImage(imageSettings, width, filePath);
    }

    private static string GetNextBiggerOrMaxSizeImage(IImageSettings imageSettings, int width, string filePath)
    {
        var fileNameTrunk = imageSettings.BaseDummyUrl;
        var fileDirectory = imageSettings.BaseDummyUrl.Split(new string[] { fileNameTrunk }, StringSplitOptions.None)
            .First();
        var fileNames =
            Directory.GetFiles(Path.Combine(Settings.ImagePath, fileDirectory), fileNameTrunk + "*.png");
        if (fileNames.Any())
        {
            var fileWidths = fileNames.Where(x => !x.Contains("s.jpg"))
                .Select(x => Convert.ToInt32(x.Split('-').Last().Replace(".png", ""))).OrderByDescending(x => x).ToList();
            var fileWidth = 0;
            for (var i = 0; i < fileWidths.Count(); i++)
            {
                if (fileWidths[i] < width)
                {
                    fileWidth = i == 0 ? fileWidths[0] : fileWidths[i - 1];
                    break;
                }
            }

            if (fileWidth == 0)
            {
                fileWidth = fileWidths.Last();
            }

            if (File.Exists(filePath + fileWidth + ".png"))
            {
                return ImageFolder + "/" + imageSettings.BaseDummyUrl + fileWidth + ".png";
            }
        }
        Logg.r.Error("Image page not available");
        return "";
    }
}