using System.Collections.Generic;
using System.Linq;

public class ProbabilityCalc_Simple2 : ProbabilityCalc_Abstract, IRegisterAsInstancePerLifetime
{
    /// <returns>CorrectnessProbability as Percentage</returns>
    public override ProbabilityCalcResult Run(IList<AnswerHistory> answerHistoryItems, Question question, User user)
    {
	    if (!answerHistoryItems.Any())
		    return ProbabilityCalcResult.GetResult(answerHistoryItems, user.CorrectnessProbability);

        var weightedFavorableOutcomes = 0m;
        var weightedTotalOutcomes = 0m;

        var index = 0;

        foreach(var historyItem in answerHistoryItems.OrderByDescending(d => d.DateCreated))
        {
            index++;
            var weight = 1m;
            if (index == 1) weight += 3;
            if (index == 2) weight += 1.8m;
            if (index == 3) weight += 1.2m;

            weightedFavorableOutcomes += 
                (historyItem.AnswerredCorrectly != AnswerCorrectness.False ? 1 : 0) * weight;
            weightedTotalOutcomes += weight;
        }

        return ProbabilityCalcResult.GetResult(
            answerHistoryItems, 
            (int)((weightedFavorableOutcomes / weightedTotalOutcomes) * 100));
    }
}