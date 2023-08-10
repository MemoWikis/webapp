using Microsoft.AspNetCore.Http;

public class LomExporter
{
    public static void AllToFileSystem(CategoryRepository categoryRepository,
        QuestionReadingRepo questionReadingRepo, 
        IHttpContextAccessor httpContextAccessor)
    {
        lock ("c6fc2ccc-bf87-4b6f-b286-d2438e117cc1")
        {
            AllToFileSystem_(categoryRepository, questionReadingRepo, httpContextAccessor);
        }    
    }

    private static void AllToFileSystem_(CategoryRepository categoryRepository,
        QuestionReadingRepo questionReadingRepo, 
        IHttpContextAccessor httpContextAccessor)
    {
        var exportPath = Settings.LomExportPath;

        if (!Directory.Exists(exportPath))
            throw new Exception("directory does not exist");

        foreach (FileInfo file in new DirectoryInfo(exportPath).GetFiles())
            file.Delete();

        ExportCategories(exportPath, categoryRepository, httpContextAccessor);
        ExportQuestions(exportPath, categoryRepository, questionReadingRepo, httpContextAccessor);
    }

    private static void ExportCategories(string exportPath, CategoryRepository categoryRepository, IHttpContextAccessor httpContextAccessor)
    {
        var listOfSelectedCategories = new[] {84, 724, 725, 841, 264, 265, 285, 606, 615, 618, 619, 607, 635, 644};
        var categories = categoryRepository.GetByIds(listOfSelectedCategories);

        foreach (var category in categories)
        {
            if (category.Creator != null && !category.Creator.IsMemuchoUser && !category.Creator.IsBeltz)
                continue;

            try
            {
                File.WriteAllText(Path.Combine(exportPath, $"topic-{category.Id}.xml"), LomXml.From(EntityCache.GetCategory(category.Id), categoryRepository));
            }
            catch (Exception e)
            {
                Logg.Error(new Exception($"Error exporting set {category.Id}", e), httpContextAccessor);
            }
        }        
    }

    private static void ExportQuestions(string exportPath,
        CategoryRepository categoryRepository, 
        QuestionReadingRepo questionReadingRepo,
        IHttpContextAccessor httpContextAccessor)
    {
        var allQuestions = questionReadingRepo.GetAll();

        foreach (var question in allQuestions)
        {
            if(question.Creator != null && !question.Creator.IsMemuchoUser && !question.Creator.IsBeltz)
                continue;

            try
            {
                File.WriteAllText(Path.Combine(exportPath, $"question-{question.Id}.xml"), LomXml.From(question, categoryRepository));
            }
            catch (Exception e)
            {
                Logg.Error(new Exception($"Error exporting question {question.Id}", e), httpContextAccessor);
            }
        }
    }
}

