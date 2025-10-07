using SkiaSharp;

public class SaveImageToFile
{
    public static void RemoveExistingAndSaveAllSizes(Stream inputStream, IImageSettings imageSettings)
    {
        var directory = Path.Combine(Settings.ImagePath, imageSettings.BasePath);

        var oldImages = Directory.GetFiles(directory, $"{imageSettings.Id}_*");

        Log.Information("SaveImageToFile.RemoveExistingAndSaveAllSizes: Removing {OldImageCount} existing images for ID {Id} in {Directory}", 
            oldImages.Length, imageSettings.Id, directory);

        foreach (var file in oldImages)
        {
            Log.Information("SaveImageToFile.RemoveExistingAndSaveAllSizes: Deleting existing file {FilePath}", file);
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
            Log.Error($"SMALL IMAGE: Original size of Image {filename} is smaller than 300px.");
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

    public static string SaveContentImageAndGetPath(Stream inputStream, IImageSettings imageSettings)
    {
        using (var image = SKBitmap.Decode(inputStream))
        {
            var guid = Guid.NewGuid();
            var filename = $"{imageSettings.ServerPathAndId()}_{guid}.jpg";
            
            Log.Information("SaveImageToFile.SaveContentImageAndGetPath: Saving new content image for ID {Id} with GUID {Guid} to {Filename}", 
                imageSettings.Id, guid, filename);
            
            using (var fileStream = File.OpenWrite(filename))
            {
                image.Encode(fileStream, SKEncodedImageFormat.Jpeg, 100);
            }

            var path = Path
                .Combine(
                    Settings.ImagePath,
                    imageSettings.BasePath,
                    $"{imageSettings.Id}_{guid}.jpg")
                .NormalizePathSeparators();
            
            var returnUrl = File.Exists(path) 
                ? $"/Images/{imageSettings.BasePath}/{imageSettings.Id}_{guid}.jpg"
                : $"/Images/${imageSettings.BaseDummyUrl}";
            
            Log.Information("SaveImageToFile.SaveContentImageAndGetPath: Returning URL {ReturnUrl} for path {Path} (exists: {FileExists})", 
                returnUrl, path, File.Exists(path));
            
            return returnUrl;
        }
    }

    public static string SaveTempQuestionContentImageAndGetPath(Stream inputStream, IImageSettings imageSettings)
    {
        using (var image = SKBitmap.Decode(inputStream))
        {
            var guid = Guid.NewGuid();

            var filename = Path.Combine(
                Settings.ImagePath,
                Settings.QuestionContentImageBasePath,
                $"tempImage_{guid}.jpg");

            using (var fileStream = File.OpenWrite(filename))
            {
                image.Encode(fileStream, SKEncodedImageFormat.Jpeg, 100);
            }

            var path = Path
                .Combine(
                    Settings.ImagePath,
                    imageSettings.BasePath,
                    $"tempImage_{guid}.jpg")
                .NormalizePathSeparators();

            if (File.Exists(path))
                return $"/Images/{imageSettings.BasePath}/tempImage_{guid}.jpg";

            return $"/Images/${imageSettings.BaseDummyUrl}";
        }
    }

    public static void RenameTempImagesWithQuestionId(string filename, int questionId)
    {
        var directory = Path.Combine(Settings.ImagePath, Settings.QuestionContentImageBasePath);
        var oldImages = Directory.GetFiles(directory, filename);

        foreach (var file in oldImages)
        {
            var newFilename = file.Replace("tempImage_", $"{questionId}_");
            File.Move(file, newFilename);
        }
    }

    public static string ReplaceTempImagePathsWithQuestionId(string text, int questionId)
    {
        var tempImagePath = $"/Images/{Settings.QuestionContentImageBasePath}/tempImage_";
        var newImagePath = $"/Images/{Settings.QuestionContentImageBasePath}/{questionId}_";
        return text.Replace(tempImagePath, newImagePath);
    }

    public static void ReplaceTempQuestionContentImages(string[] uploadedImagesInContent, Question question, QuestionWritingRepo questionWritingRepo)
    {
        var uniquePaths = uploadedImagesInContent.Distinct();

        foreach (var imageUrl in uniquePaths)
        {
            var filename = Path.GetFileName(imageUrl);
            RenameTempImagesWithQuestionId(filename, question.Id);
        }

        question.TextHtml = ReplaceTempImagePathsWithQuestionId(question.TextHtml, question.Id);

        if (question.TextExtendedHtml is { Length: > 0 })
            question.TextExtendedHtml = ReplaceTempImagePathsWithQuestionId(question.TextExtendedHtml, question.Id);

        if (question.DescriptionHtml is { Length: > 0 })
            question.DescriptionHtml = ReplaceTempImagePathsWithQuestionId(question.DescriptionHtml, question.Id);

        if (question.SolutionType == SolutionType.FlashCard)
            question.Solution = ReplaceTempImagePathsWithQuestionId(question.Solution, question.Id);

        questionWritingRepo.UpdateOrMerge(question, false);
    }
}