using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Serilog;

public class ProbabilityUpdate_Category
{
    private readonly CategoryRepository _categoryRepository;
    private readonly AnswerRepo _answerRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProbabilityUpdate_Category(CategoryRepository categoryRepository,
        AnswerRepo answerRepo,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _categoryRepository = categoryRepository;
        _answerRepo = answerRepo;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }
    public void Run()
    {
        var sp = Stopwatch.StartNew();

        foreach (var category in _categoryRepository.GetAll())
            Run(category);

        Log.Information("Calculated all category probabilities in {elapsed} ", sp.Elapsed);
    }

    public void Run(Category category)
    {
        var sp = Stopwatch.StartNew();

        var answers = _answerRepo.GetByCategories(category.Id);  

        category.CorrectnessProbability = ProbabilityCalc_Category.Run(answers);
        category.CorrectnessProbabilityAnswerCount = answers.Count;

        _categoryRepository.Update(category);

        Logg.r.Information("Calculated probability in {elapsed} for category {categoryId}", sp.Elapsed, category.Id);
    }
}