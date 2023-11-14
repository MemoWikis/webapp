using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


public class ResizeImage
{
    public static void Run(Image image, string basePathAndId, int width, bool isSquare)
    {
        if (image.Width < width)
            width = image.Width;

        if (!isSquare)
        {
            var scale = (float)width / image.Width;
            var height = (int)(image.Height * scale);
            using (var resized = new Bitmap(width, height))
            {
                using (var graphics = Graphics.FromImage(resized))
                {
                    ConfigureGraphics(graphics);
                    graphics.DrawImage(image, 0, 0, width, height);
                }

                resized.Save(basePathAndId + "_" + width + ".jpg", ImageFormat.Jpeg);
            }
            return;
        }

        //isSquare
        using (var resized = new Bitmap(width, width))
        {
            using (var graphics = Graphics.FromImage(resized))
            {
                ConfigureGraphics(graphics);
                if (image.Width > image.Height)
                {
                    var scale = (float)width / image.Height;
                    graphics.DrawImage(image, -(image.Width * scale - width) / 2, 0, image.Width * scale, width);
                }
                else
                {
                    var scale = (float)width / image.Width;
                    graphics.DrawImage(image, 0, -(image.Height * scale - width) / 2, width, image.Height * scale);
                }
            }

            resized.Save(basePathAndId + "_" + width + ImageUrl.SquareSuffix(true) + ".jpg", ImageFormat.Jpeg);
        }
    }

    public static void ConfigureGraphics(Graphics graphics)
    {
        graphics.Clear(Color.White);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
    }


}