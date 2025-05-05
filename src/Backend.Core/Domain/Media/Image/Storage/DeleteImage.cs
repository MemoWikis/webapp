public class DeleteImage
{
    public void Run(string basePath, string filename)
    {
        var directory = Path.Combine(Settings.ImagePath, basePath);
        var oldImages = Directory.GetFiles(directory, filename);

        foreach (var file in oldImages)
        {
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
        Run(Settings.PageContentImageBasePath, $"{id}_*");
        Run(Settings.PageImageBasePath, $"{id}_*");
    }

    public void RemoveAllForQuestion(int id)
    {
        Run(Settings.QuestionContentImageBasePath, $"{id}_*");
        Run(Settings.QuestionImageBasePath, $"{id}_*");
    }

    public void RunForQuestionContentImage(string filename) => Run(Settings.QuestionContentImageBasePath, filename);

    public void RunForQuestionContentImages(string[] paths)
    {
        if (paths.Length > 0)
            foreach (var path in paths)
                RunForQuestionContentImage(Path.GetFileName(path));
    }
}