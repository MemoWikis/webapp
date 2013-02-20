using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

public class StoreImages
{
    public void Run(Stream inputStream, IImageSettings imageSettings)
    {
        using (var image = Image.FromStream(inputStream))
        {
            foreach (var size in imageSettings.SizesSquare)
            {
                using (var resized = new Bitmap(size, size))
                {
                    using (var graphics = Graphics.FromImage(resized))
                    {
                        graphics.Clear(Color.White);
                        graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        if (image.Width > image.Height)
                        {
                            var scale = (float) size/image.Height;
                            graphics.DrawImage(image, -(image.Width*scale - size)/2, 0, image.Width*scale, size);
                        }
                        else
                        {
                            var scale = (float) size/image.Width;
                            graphics.DrawImage(image, 0, -(image.Height*scale - size)/2, size, image.Height*scale);
                        }
                    }
                    resized.Save(imageSettings.BasePathAndId() + "_" + size + ".jpg", ImageFormat.Jpeg);
                }
            }
            foreach (var size in imageSettings.SizesFixedWidth)
            {
                var scale = (float)size / image.Width;
                var height = (int) (image.Height * scale);
                using (var resized = new Bitmap(size, height))
                {
                    using (var graphics = Graphics.FromImage(resized))
                    {
                        graphics.Clear(Color.White);
                        graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphics.DrawImage(image, 0, 0, size, height);
                    }
                    resized.Save(imageSettings.BasePathAndId() + "_" + size + ".jpg", ImageFormat.Jpeg);
                }
            }
        }
    }
}