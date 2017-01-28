using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GetQuestionsForCategory
{
    public static IList<Question> QuestionsWithCategoryAssigned(int categoryId)
    {
        return Sl.R<QuestionRepo>().GetForCategory(categoryId);
    }

    public static IList<Question> QuestionsInSetsWithCategoryAssigned(int categoryId)
    {
        var sets = Sl.R<SetRepo>().GetForCategory(categoryId);
        return sets.SelectMany(s => s.QuestionsInSet).Select(x => x.Question).Distinct().ToList();
    }

    public static IList<Question> AllIncludingQuestionsInSet(int categoryId)
    {
        return QuestionsWithCategoryAssigned(categoryId)
                .Union(QuestionsInSetsWithCategoryAssigned(categoryId)).ToList();
    }

    public static IList<Question> QuestionsNotIncludedInSet(int categoryId)
    {
        return QuestionsWithCategoryAssigned(categoryId)
                .Except(QuestionsInSetsWithCategoryAssigned(categoryId)).ToList();
    }
}
