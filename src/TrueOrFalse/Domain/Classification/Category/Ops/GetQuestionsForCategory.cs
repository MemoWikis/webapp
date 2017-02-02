using System.Collections.Generic;
using System.Linq;

public class GetQuestionsForCategory
{
    public static IList<Question> QuestionsWithCategoryAssigned(int categoryId) => 
        Sl.R<QuestionRepo>().GetForCategory(categoryId);

    public static IList<Question> QuestionsInSetsWithCategoryAssigned(int categoryId) => 
        Sl.SetRepo
            .GetForCategory(categoryId)
            .SelectMany(s => s.QuestionsInSet)
            .Select(x => x.Question)
            .Distinct()
            .ToList();

    public static IList<Question> AllIncludingQuestionsInSet(int categoryId) => 
        QuestionsWithCategoryAssigned(categoryId)
            .Union(QuestionsInSetsWithCategoryAssigned(categoryId))
            .ToList();

    public static IList<Question> QuestionsNotIncludedInSet(int categoryId) => 
        QuestionsWithCategoryAssigned(categoryId)
            .Except(QuestionsInSetsWithCategoryAssigned(categoryId))
            .ToList();
}
