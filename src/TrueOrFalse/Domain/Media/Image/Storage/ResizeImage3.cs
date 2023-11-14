using SkiaSharp;

public class ResizeImage3
{
    public static string? RunAndReturnPath(SKBitmap originalBitmap, string basePathAndId, int width, bool isSquare)
    {
        try
        {
            var imagePath = basePathAndId + "_" + width + ImageUrl.SquareSuffix(isSquare) + ".jpg";

            using var croppedAndResizedBitmap = ResizeAndCrop(originalBitmap, width, isSquare);
            using var image = SKImage.FromBitmap(croppedAndResizedBitmap);
            using var data = image.Encode(SKEncodedImageFormat.Jpeg, 100);
            using var stream = File.OpenWrite(imagePath);
            data.SaveTo(stream);

            return imagePath;
        }
        catch (Exception ex)
        {
            Logg.Error(ex);
            return null;
        }
    }

    private static SKBitmap ResizeAndCrop(SKBitmap originalBitmap, int width, bool isSquare)
    {
        var scale = (float)width / originalBitmap.Width;
        var newHeight = (int)(originalBitmap.Height * scale);

        using var resizedBitmap = originalBitmap.Resize(new SKImageInfo(width, newHeight), SKFilterQuality.High);

        if (!isSquare) return resizedBitmap;

        var size = Math.Min(resizedBitmap.Width, resizedBitmap.Height);
        var x = (resizedBitmap.Width - size) / 2;
        var y = (resizedBitmap.Height - size) / 2;

        var croppedBitmap = new SKBitmap(size, size);
        using var canvas = new SKCanvas(croppedBitmap);

        canvas.Clear(SKColors.Transparent);
        canvas.DrawBitmap(resizedBitmap, new SKRectI(x, y, x + size, y + size), new SKRect(0, 0, size, size));
        return croppedBitmap;
    }
}