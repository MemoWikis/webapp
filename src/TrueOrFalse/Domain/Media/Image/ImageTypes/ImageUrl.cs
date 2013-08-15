using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using TrueOrFalse;

public class ImageUrl
{
    public bool HasUploadedImage;
    public string Url;

    public static ImageUrl Get(
        IImageSettings imageSettings,
        int width,
        bool isSquare,
        Func<int, string> getFallBackImage)
    {
        var requestedImagePath = imageSettings.ServerPathAndId() + "_" + width + SquareSuffix(isSquare) + ".jpg";

        if (imageSettings.Id != -1)
        {
            var resultWhenImageExists = new ImageUrl{
                Url = imageSettings.BasePath + imageSettings.Id +"_" + width + SquareSuffix(isSquare) + ".jpg",
                HasUploadedImage = true
            };

            if (File.Exists(requestedImagePath))
                return resultWhenImageExists;

            //we guess the biggest file has a width of 512
            var biggestAvailableImage = string.Format("{0}_512.jpg", imageSettings.ServerPathAndId());
            if (File.Exists(biggestAvailableImage)){
                using(var image = Image.FromFile(biggestAvailableImage)){
                    ResizeImage.Run(image, imageSettings.ServerPathAndId(), width, isSquare);
                }
                return resultWhenImageExists;    
            }

            //we search for the biggest file
            var files = Directory.GetFiles(HttpContext.Current.Server.MapPath(imageSettings.BasePath), string.Format("{0}_*.jpg", imageSettings.Id));
            if (files.Any()){
                var maxFileWidth = files.Where(x => !x.Contains("s.jpg")).Select(x => Convert.ToInt32(x.Split('_').Last().Replace(".jpg", ""))).OrderByDescending(x => x).First();
                using (var image = Image.FromFile(string.Format("{0}_{1}.jpg", imageSettings.ServerPathAndId(), maxFileWidth))){
                    ResizeImage.Run(image, imageSettings.ServerPathAndId(), width, isSquare);
                }
                return resultWhenImageExists;    
            }
            
        }

        return new ImageUrl { Url = getFallBackImage(width), HasUploadedImage = false};
    }

    public static string SquareSuffix(bool isSquare){
        return isSquare ?  "s" : "";
    }

    public ImageUrl SetSuffix(ImageMetaData imageMeta)
    {
        var urlSuffix = "";
        if (imageMeta != null)
            urlSuffix = "?" + imageMeta.DateModified.ToString("yyyyMMdd-HHMMss");

        Url += urlSuffix ;
        return this;
    }
}