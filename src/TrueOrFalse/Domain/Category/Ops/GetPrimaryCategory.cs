using System.Linq;

public class GetPrimaryCategory
{
    public static CategoryCacheItem GetForQuestion(QuestionCacheItem question)
    {
        if (question.Categories.Any())
            return question.Categories.First();

        return null;
    }
}
