using SkiaSharp;

public class SaveImageToFile
{
    public static void RemoveExistingAndSaveAllSizes(Stream inputStream, IImageSettings imageSettings)
    {
        var directory = Path.Combine(Settings.ImagePath, imageSettings.BasePath);

        var oldImages = Directory.GetFiles(directory, $"{imageSettings.Id}_*");

        foreach (var file in oldImages)
        {
            File.Delete(file);
        }

        using (var original = SKBitmap.Decode(inputStream))
        {
            SaveOriginalSize(imageSettings, original);

            foreach (var size in imageSettings.SizesSquare)
            {
                ResizeImage.RunAndReturnPath(original, imageSettings.ServerPathAndId(), size, isSquare: true);
            }

            foreach (var width in imageSettings.SizesFixedWidth)
            {
                ResizeImage.RunAndReturnPath(original, imageSettings.ServerPathAndId(), width, isSquare: false);
            }
        }
    }

    private static void SaveOriginalSize(IImageSettings imageSettings, SKBitmap image)
    {
        var filename = $"{imageSettings.ServerPathAndId()}_{image.Width}.jpg";
        using (var fileStream = File.OpenWrite(filename))
        {
            image.Encode(fileStream, SKEncodedImageFormat.Jpeg, 100);
        }

        if (image.Width < 300)
        {
            Logg.r.Error($"SMALL IMAGE: Original size of Image {filename} is smaller than 300px.");
        }
    }

    public static void SaveTempImage(Stream inputStream, TmpImage tmpImage)
    {
        using (var original = SKBitmap.Decode(inputStream))
        {
            using (var fileStream = File.OpenWrite(Path.Combine(Settings.ImagePath, tmpImage.Path)))
            {
                original.Encode(fileStream, SKEncodedImageFormat.Png, 100);
            }

            var scale = (float)tmpImage.PreviewWidth / original.Width;
            var height = (int)(original.Height * scale);

            using (var resized = new SKBitmap(tmpImage.PreviewWidth, height))
            {
                using (var canvas = new SKCanvas(resized))
                {
                    canvas.Clear(SKColors.White);
                    canvas.DrawBitmap(original, new SKRect(0, 0, tmpImage.PreviewWidth, height));
                }

                using (var fileStream = File.OpenWrite(Path.Combine(Settings.ImagePath, tmpImage.PathPreview)))
                {
                    resized.Encode(fileStream, SKEncodedImageFormat.Jpeg, 100);
                }
            }
        }
    }
}