using System.Linq;

public class GetPrimaryCategory
{
    public static CategoryCacheItem GetForQuestion(Question question)
    {
        if (question.Categories.Any())
            return CategoryCacheItem.ToCacheCategory(question.Categories.First());

        return null;
    }
}
