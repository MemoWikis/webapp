using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

public class SaveImageToFile
{
    public static void Run(Stream inputStream, IImageSettings imageSettings)
    {
        var oldImages = Directory.GetFiles(
            HttpContext.Current.Server.MapPath(imageSettings.BasePath), 
            string.Format("{0}_*", imageSettings.Id)
        );

        foreach (var file in oldImages){
            File.Delete(file);
        }

        using (var image = Image.FromStream(inputStream)){

            if (image.VerticalResolution != 96.0F || image.HorizontalResolution != 96.0F)
                ((Bitmap)image).SetResolution(96.0F, 96.0F);

            SaveOriginalSize(imageSettings, image);

            foreach (var size in imageSettings.SizesSquare){
                ResizeImage.Run(image, imageSettings.ServerPathAndId(), size, isSquare: true);
            }

            foreach (var width in imageSettings.SizesFixedWidth){//$temp: hier werden die verschiedenen Bildgroessen abgelegt
                ResizeImage.Run(image, imageSettings.ServerPathAndId(), width, isSquare: false);
            }
        }
    }

    private static void SaveOriginalSize(IImageSettings imageSettings, Image image)
    {
        using (var resized = new Bitmap(image))
        {
            using (var graphics = Graphics.FromImage(resized))
            {
                ResizeImage.ConfigureGraphics(graphics);
                graphics.DrawImage(image, 0, 0);
            }
            var filename = imageSettings.ServerPathAndId() + "_" + image.Width + ".jpg";
            resized.Save(filename, ImageFormat.Jpeg);

            if (image.Width < 300)
            {
                Logg.r().Error($"SMALL IMAGE: Original size of Image {filename} is smaller than 300px.");
            }
        }
    }


    /// <summary>store temp images</summary>
    public static void Run(Stream inputStream, TmpImage tmpImage)
    {
        using (var image = Image.FromStream(inputStream)){

            image.Save(HttpContext.Current.Server.MapPath(tmpImage.Path), ImageFormat.Png);

            var scale = (float)tmpImage.PreviewWidth / image.Width;
            var height = (int)(image.Height * scale);
            using (var resized = new Bitmap(tmpImage.PreviewWidth, height)){
                using (var graphics = Graphics.FromImage(resized)){
                    graphics.Clear(Color.White);
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.DrawImage(image, 0, 0, tmpImage.PreviewWidth, height);
                }
                resized.Save(HttpContext.Current.Server.MapPath(tmpImage.PathPreview), ImageFormat.Jpeg);
            }
        }
    }
}