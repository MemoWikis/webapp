public class DeleteImage
{
    public void Run(string basePath, string filename)
    {
        var directory = Path.Combine(Settings.ImagePath, basePath);
        var oldImages = Directory.GetFiles(directory, filename);

        Log.Information("DeleteImage.Run: Attempting to delete {FileCount} files with pattern {Pattern} in directory {Directory}", 
            oldImages.Length, filename, directory);

        foreach (var file in oldImages)
        {
            Log.Information("DeleteImage.Run: Deleting file {FilePath}", file);
            File.Delete(file);
        }
    }

    public void Run(string basePath, IList<string> filenames)
    {
        foreach (var filename in filenames)
            Run(basePath, filename);
    }

    public void RemoveAllForPage(int id)
    {
        Log.Information("DeleteImage.RemoveAllForPage: Removing all images for page {PageId}", id);
        Run(Settings.PageContentImageBasePath, $"{id}_*");
        Run(Settings.PageImageBasePath, $"{id}_*");
    }

    public void RemoveAllForQuestion(int id)
    {
        Log.Information("DeleteImage.RemoveAllForQuestion: Removing all images for question {QuestionId}", id);
        Run(Settings.QuestionContentImageBasePath, $"{id}_*");
        Run(Settings.QuestionImageBasePath, $"{id}_*");
    }

    public void RunForQuestionContentImage(string filename)
    {
        Log.Information("DeleteImage.RunForQuestionContentImage: Deleting question content image {Filename}", filename);
        Run(Settings.QuestionContentImageBasePath, filename);
    }

    public void RunForQuestionContentImages(string[] paths)
    {
        if (paths.Length > 0)
        {
            Log.Information("DeleteImage.RunForQuestionContentImages: Deleting {Count} question content images: {Paths}", 
                paths.Length, string.Join(", ", paths));
            foreach (var path in paths)
                RunForQuestionContentImage(Path.GetFileName(path));
        }
    }
}