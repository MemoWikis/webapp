﻿using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Correctness probability for user
/// </summary>
public class ProbabilityCalc : IRegisterAsInstancePerLifetime
{
    /// <returns>CorrectnessProbability as Percentage</returns>
    public int Run(IList<AnswerHistory> answerHistoryItems)
    {
        if (!answerHistoryItems.Any())
            return -1;

        var weightedFavorableOutcomes = 0m;
        var weightedTotalOutcomes = 0m;

        var index = 0;

        foreach(var historyItem in answerHistoryItems.OrderByDescending(d => d.DateCreated))
        {
            index++;
            var weight = 1m;
            if (index == 1) weight += 5;
            if (index == 2) weight += 2.5m;
            if (index == 3) weight += 1.5m;

            weightedFavorableOutcomes += 
                (historyItem.AnswerredCorrectly != AnswerCorrectness.False ? 1 : 0) * weight;
            weightedTotalOutcomes += weight;
        }

        return (int) ((weightedFavorableOutcomes / weightedTotalOutcomes) * 100);
    }
}