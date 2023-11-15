using Microsoft.AspNetCore.Http;
using SkiaSharp;


public class ImageUrl
{
    public bool HasUploadedImage;
    public string? Url;
    private readonly HttpContext? _httpContext;
    private static string ImagePath => Settings.ImagePath;
    private static string _imageFolder = "/Images";

    public ImageUrl(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext;
    }

    private class ImagePathBuilder
    {
        private readonly string _basePath;
        private readonly int _id;
        private readonly bool _isSquare;

        public ImagePathBuilder(string basePath, int id, bool isSquare)
        {
            _basePath = basePath;
            _id = id;
            _isSquare = isSquare;
        }

        public string GetImagePath(int requestedWidth)
        {
            var fileName = $"{_id}_{requestedWidth}{SquareSuffix(_isSquare)}.jpg";
            return Path.Combine(ImagePath, _basePath, fileName).NormalizePathSeparators();
        }
    }

    public string UrlWithoutTime()
    {
        return Url.Split('?').First();
    } 

    public ImageUrl Get(
        IImageSettings imageSettings,
        int originallyRequestedWidth,
        bool isSquare,
        Func<int, string> getFallBackImage)
    { 
        var requestedWidth = originallyRequestedWidth;
        var imagePathBuilder = new ImagePathBuilder(imageSettings.BasePath, imageSettings.Id, isSquare);
        var requestedImagePath = imagePathBuilder.GetImagePath(requestedWidth);

        if (imageSettings.Id > 0)
        {
            if (File.Exists(requestedImagePath))
                return GetResult(imageSettings, requestedWidth, isSquare);

            //Requested image/width doesn't exist:

            //we guess the biggest file has a width of 512
            var image512 = Path.Combine(ImagePath, "_512.jpg");
            string? resizePath;
            if (File.Exists(image512))
            {
                using (var input = File.OpenRead(image512))
                using (var inputStream = new SKManagedStream(input))
                using (var image = SKBitmap.Decode(inputStream))
                {
                    resizePath = ResizeImage3.RunAndReturnPath(image, imageSettings.ServerPathAndId(), requestedWidth, isSquare);
                    Console.WriteLine($"Resize Image1, {resizePath}");
                    Console.WriteLine($"Requested Image Path, {requestedImagePath}");
                }

                if (File.Exists(resizePath))
                {
                    return GetResult(imageSettings, requestedWidth, isSquare);
                }
            }

            if (_httpContext == null)
            {
                Url = getFallBackImage(requestedWidth);
                HasUploadedImage = false;
                return this;
            }

            var searchPattern = $"{imageSettings.Id}_*.jpg";
            
            var basePath = Path.Combine(ImagePath, imageSettings.BasePath).NormalizePathSeparators();
            if (Directory.Exists(basePath) == false)
            {
                Logg.r.Error("Directory is not available");
            }
            var maxFileWidth = GetMaxFileWidth(basePath, searchPattern);

            if (maxFileWidth > 0){
                var imagePath = $"{imageSettings.ServerPathAndId()}_{maxFileWidth}.jpg";
                if (!File.Exists(imagePath)) return null;
                using var input = File.OpenRead(imagePath);
                using var inputStream = new SKManagedStream(input);
                var biggestAvailableImage = SKBitmap.Decode(inputStream);

                if(biggestAvailableImage != null){
                    
                    if (biggestAvailableImage.Width < requestedWidth)//if requested width is bigger than max. available width
                    {
                        var absoluteUri = $"{_httpContext.Request.Scheme}://{_httpContext.Request.Host}{_httpContext.Request.Path}{_httpContext.Request.QueryString}";
                        Logg.r.Warning($"Requested image width of {requestedWidth}px is greater than max. available {biggestAvailableImage.Width}px of image {imageSettings.ServerPathAndId()} (requested url: {absoluteUri}). ");

                        if (isSquare)
                        {
                            requestedWidth = Math.Max(biggestAvailableImage.Width, biggestAvailableImage.Height);

                            if (File.Exists(imageSettings.ServerPathAndId() + "_" + requestedWidth + SquareSuffix(true) + ".jpg"))
                                return GetResult(imageSettings, requestedWidth, true);

                            resizePath = ResizeImage3.RunAndReturnPath(biggestAvailableImage, imageSettings.ServerPathAndId(), requestedWidth, true);
                            Console.WriteLine($"Resize Image2, {resizePath}");
                            Console.WriteLine($"Requested Image Path, {requestedImagePath}");
                        }
                        else
                        {
                            requestedWidth = biggestAvailableImage.Width;
                        }
                    }
                    else
                    {
                        resizePath = ResizeImage3.RunAndReturnPath(biggestAvailableImage, imageSettings.ServerPathAndId(), requestedWidth, isSquare);
                        Console.WriteLine($"Resize Image3, {resizePath}");
                        Console.WriteLine($"Requested Image Path, {requestedImagePath}");
                    }

                }
                if(File.Exists(imagePathBuilder.GetImagePath(requestedWidth)))
                {
                    return GetResult(imageSettings, requestedWidth, isSquare);
                }
                if(File.Exists(imagePathBuilder.GetImagePath(maxFileWidth)))
                {
                    return GetResult(imageSettings, maxFileWidth, isSquare);
                }
            }
        }

        var url = Path.Combine(_imageFolder, getFallBackImage(originallyRequestedWidth));
        Url =  url.NormalizePathSeparators();
        HasUploadedImage = false;
        return this;
    }

    private SKBitmap? LoadImage(string pathAndId, int fileWidth)
    {
        var imagePath = $"{pathAndId}_{fileWidth}.jpg";
        if (!File.Exists(imagePath)) return null;
        using var input = File.OpenRead(imagePath);
        using var inputStream = new SKManagedStream(input);
        return SKBitmap.Decode(inputStream);
    }

    private int GetMaxFileWidth(string basePath, string searchPattern)
    {
        var fileNames = Directory.GetFiles(basePath, searchPattern);

        return fileNames
            .Where(x => !x.Contains("s.jpg"))
            .Select(x => Convert.ToInt32(x.Split('_').Last().Replace(".jpg", "")))
            .OrderByDescending(x => x)
            .FirstOrDefault();
    }

    private ImageUrl GetResult(IImageSettings imageSettings, int width, bool isSquare)
    {
        var basePath = $"/Images/{imageSettings.BasePath}/{imageSettings.Id}"; 
        var url = width ==  -1 ?
                             basePath + ".jpg" :
                            basePath + "_" + width + SquareSuffix(isSquare) + ".jpg";
        Url = url + "?t=" + File.GetLastWriteTime(Path.Combine(Settings.ImagePath, url))
            .ToString("yyyyMMddhhmmss");
        Console.WriteLine($"GetResult, {Url}");
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
                return _imageFolder + "/" + imageSettings.BaseDummyUrl + fileWidth + ".png";
            }
        }
        Logg.r.Error("Image page not available");
        return "";
    }
}