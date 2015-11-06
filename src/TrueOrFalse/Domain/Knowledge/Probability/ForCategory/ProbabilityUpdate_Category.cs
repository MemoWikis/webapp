using System.Diagnostics;

public class ProbabilityUpdate_Category
{
    public static void Run()
    {
        var sp = Stopwatch.StartNew();

        foreach (var category in Sl.R<CategoryRepository>().GetAll())
            Run(category);

        Logg.r().Information("Calculated all category probabilities in {elapsed} ", sp.Elapsed);
    }

    public static void Run(Category category)
    {
        var sp = Stopwatch.StartNew();

        var answers = Sl.R<AnswerRepo>().GetByCategories(category.Id);  

        category.CorrectnessProbability = ProbabilityCalc_Category.Run(answers);
        category.CorrectnessProbabilityAnswerCount = answers.Count;

        Sl.R<CategoryRepository>().Update(category);

        Logg.r().Information("Calculated probability in {elapsed} for category {categoryId}", sp.Elapsed, category.Id);
    }
}