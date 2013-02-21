using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

public class StoreImages
{
    public static void Run(Stream inputStream, IImageSettings imageSettings)
    {
        using (var image = Image.FromStream(inputStream)){
            
            image.Save(imageSettings.BasePathAndId() + ".jpg", ImageFormat.Jpeg);

            foreach (var size in imageSettings.SizesSquare){
                using (var resized = new Bitmap(size, size)){
                    using (var graphics = Graphics.FromImage(resized)){
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

            foreach (var width in imageSettings.SizesFixedWidth){
                var scale = (float)width / image.Width;
                var height = (int) (image.Height * scale);
                using (var resized = new Bitmap(width, height)){
                    using (var graphics = Graphics.FromImage(resized)){
                        graphics.Clear(Color.White);
                        graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphics.DrawImage(image, 0, 0, width, height);
                    }
                    resized.Save(imageSettings.BasePathAndId() + "_" + width + ".jpg", ImageFormat.Jpeg);
                }
            }
        }
    }

    /// <summary>store temp images</summary>
    public static void Run(Stream inputStream, TmpImage tmpImage)
    {
        using (var image = Image.FromStream(inputStream)){

            image.Save(HttpContext.Current.Server.MapPath(tmpImage.Path), ImageFormat.Jpeg);

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