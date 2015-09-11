public class ProbabilityUpdate_Category
{
    public static void Run()
    {
        foreach (var category in Sl.R<CategoryRepository>().GetAll())
            Run(category);
    }

    public static void Run(Category category)
    {
        var answerHistoryItems = Sl.R<AnswerHistoryRepository>().GetByCategories(category.Id);

        category.CorrectnessProbability =
            ProbabilityCalc_Category.Run(answerHistoryItems);

        Sl.R<CategoryRepository>().Update(category);
    }
}