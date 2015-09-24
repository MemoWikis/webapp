using System;
using System.Collections.Generic;
using System.Linq;

public class GetStreaks : IRegisterAsInstancePerLifetime
{
    public GetStreaksResult Run(User user, bool onlyLearningSessions = false)
    {
        var getAnswerStats = Sl.R<GetAnswerStatsInPeriod>().Run(
            user.Id, 
            user.DateCreated, 
            DateTime.Now.AddMinutes(1), 
            groupByDate :true,
            onlyLearningSessions: false);

        var result = new GetStreaksResult();
        result.TotalLearningDays = getAnswerStats.Count;

        if (getAnswerStats.Count <= 1)
            return result;

        SetLastStreak(getAnswerStats, result);
        SetLongestStreak(getAnswerStats, result);

        return result;
    }

    private static void SetLastStreak(IList<GetAnswerStatsInPeriodResult> getAnswerStats, GetStreaksResult result)
    {
        if (getAnswerStats.Last().DateTime.Date != DateTime.Now.Date)
            return;

        GetAnswerStatsInPeriodResult previousItem = getAnswerStats.Last();
        for (var i = getAnswerStats.Count - 1; i >= 0; i--)
        {
            if (i == getAnswerStats.Count - 1)
                continue;

            if (previousItem.DateTime.Date == getAnswerStats[i].DateTime.Date.AddDays(1))
                previousItem = getAnswerStats[i];
            else
            {
                result.LastStart = previousItem.DateTime.Date;
                result.LastEnd = getAnswerStats.Last().DateTime.Date;
                result.LastLength = (int)((result.LastEnd - result.LastStart).TotalDays) + 1;
                break;
            }
        }
    }

    private static void SetLongestStreak(IList<GetAnswerStatsInPeriodResult> getAnswerStats, GetStreaksResult result)
    {
        GetAnswerStatsInPeriodResult previousItem = getAnswerStats[0];
        result.LongestStart = getAnswerStats[0].DateTime.Date;
        result.LongestEnd = getAnswerStats[0].DateTime.Date;
        result.LongestLength = 0;

        var currentStreakStart = getAnswerStats[0].DateTime.Date;
        var currentStreakEnd = getAnswerStats[0].DateTime.Date;

        foreach (var answerStatItem in getAnswerStats.Skip(1))
        {
            if (previousItem.DateTime.Date == answerStatItem.DateTime.Date.AddDays(-1))
            {
                currentStreakEnd = answerStatItem.DateTime.Date;
            }
            else
            {
                var lastStreakLength = (int) ((currentStreakEnd - currentStreakStart).TotalDays);
                if (lastStreakLength > result.LongestLength)
                {
                    result.LongestStart = currentStreakStart;
                    result.LongestEnd = currentStreakEnd;
                    result.LongestLength = lastStreakLength;
                }

                currentStreakStart = answerStatItem.DateTime.Date;
                currentStreakEnd = answerStatItem.DateTime.Date;
            }

            previousItem = answerStatItem;
        }

        result.LongestLength = result.LongestLength + 1;
    }
}