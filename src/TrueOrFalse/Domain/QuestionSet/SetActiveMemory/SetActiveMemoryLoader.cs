using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueOrFalse;

public class SetActiveMemoryLoader
{
    public static SetActiveMemory Run(Set set, IList<QuestionValuation> questionValuationsForUser)
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

