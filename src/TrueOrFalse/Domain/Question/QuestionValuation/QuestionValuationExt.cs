using System.Collections.Generic;
using System.Linq;

public static class QuestionValuationExt
{
    public static QuestionValuation ByQuestionId(this IEnumerable<QuestionValuation> questionValuations, int questionId)
    {
        return questionValuations.FirstOrDefault(x => x.Question.Id == questionId);
    }

    public static QuestionValuationCacheItem ByQuestionId(
        this IEnumerable<QuestionValuationCacheItem> questionValuations,
        int questionId)
    {
        return questionValuations.FirstOrDefault(x => x.Question.Id == questionId);
    }

    public static IList<int>
        QuestionIds(
            this IEnumerable<QuestionValuationCacheItem>
                setValuations) //todo (DaMa) QuestionValuationCacheItem setValuations why?? copy Error ?? 
    {
        return setValuations.Select(x => x.Question.Id).ToList();
    }
}