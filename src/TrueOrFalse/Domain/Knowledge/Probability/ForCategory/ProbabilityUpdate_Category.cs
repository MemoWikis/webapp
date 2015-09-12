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

        var answerHistoryItems = Sl.R<AnswerHistoryRepository>().GetByCategories(category.Id);  

        category.CorrectnessProbability = ProbabilityCalc_Category.Run(answerHistoryItems);
        category.CorrectnessProbabilityAnswerCount = answerHistoryItems.Count;

        Sl.R<CategoryRepository>().Update(category);

        Logg.r().Information("Calculated probability in {elapsed} for category {categoryId}", sp.Elapsed, category.Id);
    }
}