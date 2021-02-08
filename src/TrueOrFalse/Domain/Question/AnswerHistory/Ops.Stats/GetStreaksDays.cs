using System;
using System.Collections.Generic;
using System.Linq;

public class GetStreaksDays : IRegisterAsInstancePerLifetime
{
    public GetStreaksDaysResult Run(
        User user, 
        bool onlyLearningSessions = false,
        int? startHour = null,
        int? endHour = null,
        DateTime? seperateStreakInRecentPeriodSince = null)
    {
        var getAnswerStats = Sl.R<GetAnswerStatsInPeriod>().Run(
            user.Id, 
            user.DateCreated, 
            DateTime.Now.AddMinutes(1), 
            groupByDate :true,
            onlyLearningSessions: onlyLearningSessions, 
            startHour: startHour,
            endHour: endHour,
            excludeAnswerViews: true);

        var result = new GetStreaksDaysResult();
        result.TotalLearningDays = getAnswerStats.Count;

        if (getAnswerStats.Count == 0)
            return result;

        getAnswerStats = getAnswerStats.OrderBy(x => x.DateTime).ToList();

        SetLastStreak(getAnswerStats, result);
        SetLongestStreak(getAnswerStats, result);
        if (seperateStreakInRecentPeriodSince.HasValue)
            SetLongestStreakInRecentPeriod(getAnswerStats, result, (DateTime) seperateStreakInRecentPeriodSince);

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
        if (!getAnswerStats.Any())
            return;

        GetAnswerStatsInPeriodResult previousItem = getAnswerStats[0];
        daysResult.LongestStart = getAnswerStats[0].DateTime.Date;
        daysResult.LongestEnd = getAnswerStats[0].DateTime.Date;
        daysResult.LongestLength = 0;

        var currentStreakStart = getAnswerStats[0].DateTime.Date;

        foreach (var answerStatItem in getAnswerStats.Skip(1))
        {
            if (previousItem.DateTime.Date == answerStatItem.DateTime.Date.AddDays(-1))
            {
                var currentStreakEnd = answerStatItem.DateTime.Date;

                var lastStreakLength = (int)((currentStreakEnd - currentStreakStart).TotalDays);
                if (lastStreakLength >= daysResult.LongestLength)
                {
                    daysResult.LongestStart = currentStreakStart;
                    daysResult.LongestEnd = currentStreakEnd;
                    daysResult.LongestLength = lastStreakLength;
                }
            }
            else
            {
                currentStreakStart = answerStatItem.DateTime.Date;
            }

            previousItem = answerStatItem;
        }

        daysResult.LongestLength = daysResult.LongestLength + 1;

        if ((daysResult.LongestLength == 1) && (currentStreakStart != daysResult.LongestStart))
        {
            daysResult.LongestStart = daysResult.LongestEnd = currentStreakStart;
        }
    }

    private static void SetLongestStreakInRecentPeriod(IList<GetAnswerStatsInPeriodResult> getAnswerStats, GetStreaksDaysResult daysResult, DateTime since)
    {
        var recentResult = new GetStreaksDaysResult();
        var answerStatsSince = getAnswerStats.Where(d => d.DateTime.Date >= since.Date).ToList();
        if (answerStatsSince.Count == 0)
        {
            return;
        }

        SetLongestStreak(answerStatsSince, recentResult);
        daysResult.RecentPeriodSince = since.Date;
        daysResult.RecentPeriodSLongestStart = recentResult.LongestStart;
        daysResult.RecentPeriodSLongestEnd = recentResult.LongestEnd;
        daysResult.RecentPeriodSLongestLength = recentResult.LongestLength;
    }
}