using SkiaSharp;

public class ResizeImage
{
    public static string? RunAndReturnPath(SKBitmap originalBitmap, string basePathAndId, int width, bool isSquare)
    {
        try
        {
            if (originalBitmap.Width < width)
                width = originalBitmap.Width;

            var height = width;
            if (!isSquare)
            {
                var scale = (float)width / originalBitmap.Width;
                height = (int)(originalBitmap.Height * scale);
            }

            using var resizedImage = new SKBitmap(width, height);
            using (var canvas = new SKCanvas(resizedImage))
            {
                ConfigureCanvas(canvas);

                var destRect = new SKRect(0, 0, width, height);
                if (isSquare && originalBitmap.Width > originalBitmap.Height)
                {
                    float scale = (float)width / originalBitmap.Height;
                    float scaledWidth = originalBitmap.Width * scale;
                    destRect = new SKRect(-(scaledWidth - width) / 2, 0, scaledWidth - (scaledWidth - width) / 2, height);
                }
                else if (isSquare)
                {
                    float scale = (float)width / originalBitmap.Width;
                    float scaledHeight = originalBitmap.Height * scale;
                    destRect = new SKRect(0, -(scaledHeight - height) / 2, width, scaledHeight - (scaledHeight - height) / 2);
                }

                canvas.DrawBitmap(originalBitmap, destRect,
                    paint: new SKPaint
                    {
                        IsAntialias = true,
                        FilterQuality = SKFilterQuality.High,
                        IsDither = true
                    });
            }

            // Save the image
            var imagePath = basePathAndId + "_" + width + ImageUrl.SquareSuffix(true) + ".jpg";
            using (var image = SKImage.FromBitmap(resizedImage))
            using (var data = image.Encode(SKEncodedImageFormat.Jpeg, 100))
            {
                using (var stream = File.OpenWrite(imagePath))
                {
                    data.SaveTo(stream);
                }
            }
            return imagePath; // Return the path to the saved image
        }
        catch (Exception ex)
        {
            Logg.Error(ex);
            return null;
        }
    }

    private static void ConfigureCanvas(SKCanvas canvas)
    {
        canvas.Clear(SKColors.White);
    }
}