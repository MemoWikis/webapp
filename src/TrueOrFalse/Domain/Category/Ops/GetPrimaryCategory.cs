using System.Linq;

public class GetPrimaryCategory
{
    public static CategoryCacheItem GetForQuestion(Question question)
    {
        if (question.CategoriesIds.Any())
            return CategoryCacheItem.ToCacheCategory(question.CategoriesIds.First());

        return null;
    }
}
