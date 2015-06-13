using System.Collections.Generic;
using System.Linq;

public static class QuestionValuationExt
{
    public static QuestionValuation ByQuestionId(this IEnumerable<QuestionValuation> questionValuations, int questionId)
    {
        return questionValuations.FirstOrDefault(x =>  x.Question.Id == questionId);
    }

    public static IList<int> QuestionIds(this IEnumerable<QuestionValuation> setValuations)
    {
        return setValuations.Select(x => x.Question.Id).ToList();
    }
}