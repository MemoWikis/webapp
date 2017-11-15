using System.Linq;

public class ReferenceCount
{
    public static int Get(int categorId)
    {
        return Sl.Session
            .QueryOver<Reference>()
            .Where(x => x.Category.Id == categorId)
            .RowCount();
    }

    public static int GetInclCategorizedQuestions(Category category)
    {
        var questionIds = category.GetAggregatedQuestionsFromMemoryCache().Select(q => q.Id).ToList();
        var questionsWithReferenceIds = Sl.Session
            .QueryOver<Reference>()
            .Where(x => x.Category.Id == category.Id)
            .Select(r => r.Question.Id)
            .List<int>();
        questionIds.AddRange(questionsWithReferenceIds);
        questionIds = questionIds.Distinct().ToList();
        return questionIds.Count;
    }
}