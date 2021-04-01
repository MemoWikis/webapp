using System.Collections.Generic;
using System.Linq;
using TrueOrFalse;

public class SetActiveMemoryLoader
{
    public static SetActiveMemory Run(Set set, IList<QuestionValuationCacheItem> questionValuationsForUser)
    {
        return new SetActiveMemory
        {
            TotalQuestions = set.QuestionsInSet.Count,
            TotalInActiveMemory = set.QuestionsInSet.Where(x =>
            {
                var valuation = questionValuationsForUser.ByQuestionId(x.Question.Id);
                if (valuation == null)
                    return false;

                return (valuation.CorrectnessProbability > 90);
            }).Count()
        };
    }
}

