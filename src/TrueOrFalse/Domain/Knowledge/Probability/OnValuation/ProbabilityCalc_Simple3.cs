﻿using System.Collections.Generic;
using System.Linq;

public class ProbabilityCalc_Simple3 : ProbabilityCalc_Abstract, IRegisterAsInstancePerLifetime
{
    /// <returns>CorrectnessProbability as Percentage</returns>
    public override ProbabilityCalcResult Run(IList<AnswerHistory> answerHistoryItems)
    {
	    if (!answerHistoryItems.Any())
		    return ProbabilityCalcResult.GetResult(answerHistoryItems, -1);

        var weightedFavorableOutcomes = 0m;
        var weightedTotalOutcomes = 0m;

        var index = 0;

        foreach(var historyItem in answerHistoryItems.OrderByDescending(d => d.DateCreated))
        {
            index++;
            var weight = 1m;
            if (index == 1) weight += 2;
            if (index == 2) weight += 1.3m;
            if (index == 3) weight += 1.1m;

            weightedFavorableOutcomes += 
                (historyItem.AnswerredCorrectly != AnswerCorrectness.False ? 1 : 0) * weight;
            weightedTotalOutcomes += weight;
        }

        return ProbabilityCalcResult.GetResult(
            answerHistoryItems, 
            (int)((weightedFavorableOutcomes / weightedTotalOutcomes) * 100));
    }
}