using System;
using System.Collections.Generic;
using System.Linq;

public class GetStreaksDays : IRegisterAsInstancePerLifetime
{
    public GetStreaksDaysResult Run(
        User user, 
        bool onlyLearningSessions = false,
        int? startHour = null,
        int? endHour = null)
    {
        var getAnswerStats = Sl.R<GetAnswerStatsInPeriod>().Run(
            user.Id, 
            user.DateCreated, 
            DateTime.Now.AddMinutes(1), 
            groupByDate :true,
            onlyLearningSessions: onlyLearningSessions, 
            startHour: startHour,
            endHour: endHour);

        var result = new GetStreaksDaysResult();
        result.TotalLearningDays = getAnswerStats.Count;

        if (getAnswerStats.Count <= 1)
            return result;

        SetLastStreak(getAnswerStats, result);
        SetLongestStreak(getAnswerStats, result);

        return result;
    }

    private static void SetLastStreak(IList<GetAnswerStatsInPeriodResult> getAnswerStats, GetStreaksDaysResult daysResult)
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
                daysResult.LastStart = previousItem.DateTime.Date;
                daysResult.LastEnd = getAnswerStats.Last().DateTime.Date;
                daysResult.LastLength = (int)((daysResult.LastEnd - daysResult.LastStart).TotalDays) + 1;
                break;
            }
        }
    }

    private static void SetLongestStreak(IList<GetAnswerStatsInPeriodResult> getAnswerStats, GetStreaksDaysResult daysResult)
    {
        GetAnswerStatsInPeriodResult previousItem = getAnswerStats[0];
        daysResult.LongestStart = getAnswerStats[0].DateTime.Date;
        daysResult.LongestEnd = getAnswerStats[0].DateTime.Date;
        daysResult.LongestLength = 0;

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
                if (lastStreakLength > daysResult.LongestLength)
                {
                    daysResult.LongestStart = currentStreakStart;
                    daysResult.LongestEnd = currentStreakEnd;
                    daysResult.LongestLength = lastStreakLength;
                }

                currentStreakStart = answerStatItem.DateTime.Date;
                currentStreakEnd = answerStatItem.DateTime.Date;
            }

            previousItem = answerStatItem;
        }

        daysResult.LongestLength = daysResult.LongestLength + 1;
    }
}