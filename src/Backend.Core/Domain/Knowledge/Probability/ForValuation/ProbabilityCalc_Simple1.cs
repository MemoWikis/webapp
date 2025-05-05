public class ProbabilityCalc_Simple1 : ProbabilityCalc_Abstract, IRegisterAsInstancePerLifetime
{
    /// <returns>CorrectnessProbability as Percentage</returns>
    public override ProbabilityCalcResult Run(
        IList<Answer> previousHistoryItems,
        QuestionCacheItem question,
        UserCacheItem user)
    {
        if (!previousHistoryItems.Any())
            return ProbabilityCalcResult.GetResult(previousHistoryItems,
                question.CorrectnessProbability);

        var weightedFavorableOutcomes = 0m;
        var weightedTotalOutcomes = 0m;

        var index = 0;

        foreach (var historyItem in previousHistoryItems.OrderByDescending(d => d.DateCreated))
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

        return ProbabilityCalcResult.GetResult(
            previousHistoryItems,
            (int)((weightedFavorableOutcomes / weightedTotalOutcomes) * 100));
    }
}