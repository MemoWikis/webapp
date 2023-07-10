using System.Diagnostics;

public class ProbabilityUpdate_Category
{
    public static void Run(CategoryRepository categoryRepository, AnswerRepo answerRepo)
    {
        var sp = Stopwatch.StartNew();

        foreach (var category in categoryRepository.GetAll())
            Run(category, categoryRepository, answerRepo);

        Logg.r().Information("Calculated all category probabilities in {elapsed} ", sp.Elapsed);
    }

    public static void Run(Category category, CategoryRepository categoryRepository, AnswerRepo answerRepo)
    {
        var sp = Stopwatch.StartNew();

        var answers = answerRepo.GetByCategories(category.Id);  

        category.CorrectnessProbability = ProbabilityCalc_Category.Run(answers);
        category.CorrectnessProbabilityAnswerCount = answers.Count;

        categoryRepository.Update(category);

        Logg.r().Information("Calculated probability in {elapsed} for category {categoryId}", sp.Elapsed, category.Id);
    }
}