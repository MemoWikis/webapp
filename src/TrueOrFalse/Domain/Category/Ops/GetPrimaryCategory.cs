using System.Linq;

public class GetPrimaryCategory
{
    public static CategoryCacheItem GetForQuestion(Question question)
    {
        if (question.Categories.Any())
            return EntityCache.GetCategoryCacheItem(question.Categories.First().Id);

        return null;
    }
}
