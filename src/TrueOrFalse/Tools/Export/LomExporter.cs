using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;

public class LomExporter
{
    private readonly CategoryRepository _categoryRepository;
    private readonly QuestionReadingRepo _questionReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IActionContextAccessor _actionContextAccessor;

    public LomExporter(CategoryRepository categoryRepository,
        QuestionReadingRepo questionReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
         IActionContextAccessor actionContextAccessor)
    {
        _categoryRepository = categoryRepository;
        _questionReadingRepo = questionReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _actionContextAccessor = actionContextAccessor;
    }
    public void AllToFileSystem()
    {
        lock ("c6fc2ccc-bf87-4b6f-b286-d2438e117cc1")
        {
            AllToFileSystem_();
        }
    }

    private void AllToFileSystem_()
    {
        var exportPath = Settings.LomExportPath;

        if (!Directory.Exists(exportPath))
            throw new Exception("directory does not exist");

        foreach (FileInfo file in new DirectoryInfo(exportPath).GetFiles())
            file.Delete();

        ExportCategories(exportPath);
        ExportQuestions(exportPath);
    }

    private void ExportCategories(string exportPath)
    {
        var listOfSelectedCategories = new[] { 84, 724, 725, 841, 264, 265, 285, 606, 615, 618, 619, 607, 635, 644 };
        var categories = _categoryRepository.GetByIds(listOfSelectedCategories);

        foreach (var category in categories)
        {
            if (category.Creator != null && !category.Creator.IsMemuchoUser && !category.Creator.IsBeltz)
                continue;

            try
            {
                File.WriteAllText(
                    Path.Combine(exportPath, $"topic-{category.Id}.xml"), 
                    LomXml.From(EntityCache.GetCategory(category.Id), 
                        _categoryRepository, 
                        _httpContextAccessor,
                        _actionContextAccessor));
            }
            catch (Exception e)
            {
                new Logg(_httpContextAccessor, _webHostEnvironment).Error(new Exception($"Error exporting set {category.Id}", e));
            }
        }
    }

    private void ExportQuestions(string exportPath)
    {
        var allQuestions = _questionReadingRepo.GetAll();

        foreach (var question in allQuestions)
        {
            if (question.Creator != null && !question.Creator.IsMemuchoUser && !question.Creator.IsBeltz)
                continue;

            try
            {
                File.WriteAllText(
                    Path.Combine(exportPath, $"question-{question.Id}.xml"),
                    LomXml.From(question, 
                    _categoryRepository, 
                    _httpContextAccessor, 
                    _actionContextAccessor));
            }
            catch (Exception e)
            {
                new Logg(_httpContextAccessor, _webHostEnvironment).Error(
                    new Exception($"Error exporting question {question.Id}", e));
            }
        }
    }
}

