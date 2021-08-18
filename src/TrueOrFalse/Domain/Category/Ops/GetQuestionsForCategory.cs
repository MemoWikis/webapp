using System.Collections.Generic;
using System.Linq;

public class GetQuestionsForCategory
{
    public static IList<Question> QuestionsWithCategoryAssigned(int categoryId) => 
        Sl.R<QuestionRepo>().GetForCategory(categoryId);
}
