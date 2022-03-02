using System.Collections.Generic;
using System.Linq;

public class GetQuestionsForCategory
{
    public static IList<QuestionCacheItem> QuestionsWithCategoryAssigned(int categoryId) => 
        EntityCache.GetQuestionsForCategory(categoryId);
}
