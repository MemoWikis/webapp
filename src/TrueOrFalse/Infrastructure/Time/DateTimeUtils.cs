public static class DateTimeUtils
{
    public static DateTime FirstDayOfThisWeek()
    {
        return DateTime.Now.Date.AddDays(-(int)DateTime.Now.Date.DayOfWeek + 1);
    }

    public static DateTime FirstDayOfLastWeek()
    {
        return DateTime.Now.Date.AddDays(-(int)DateTime.Now.Date.DayOfWeek - 6);
    }

    public static DateTime FirstDayOfThisMonth()
    {
        return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
    }

    public static DateTime FirstDayOfThisYear()
    {
        return new DateTime(DateTime.Now.Year, 1, 1);
    }

    public static DateTime FirstDayOfLastYear()
    {
        return new DateTime(DateTime.Now.Year - 1, 1, 1);
    }

    public static DateTime FirstDayOfLastMonth()
    {
        return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
    }

    public static string TimeElapsedAsText(DateTime dateTimeBegin)
    {
        return TrueOrFalse.TimeElapsedAsText.Run(dateTimeBegin);
    }

    public static bool IsToday(CategoryCacheItem topic)
    {
        return IsToday(topic.DateCreated);
    }

    public static bool IsRegisterToday(UserCacheItem user)
    {
        return IsToday(user.DateCreated);
    }

    private static bool IsToday(DateTime date)
    {
        return date.Date == DateTime.Now.Date;
    }

    public static List<BaseView> EnsureLastDaysIncluded(List<BaseView> topicViews, int daysCount)
    {
        var lastDays = Enumerable.Range(0, daysCount)
            .Select(i => DateTime.Now.Date.AddDays(-i))
            .ToList();

        var missingDates = lastDays.Where(date => !topicViews.Any(tv => tv.Date == date))
            .Select(date => new BaseView
            {
                Date = date,
                Views = 0
            })
            .ToList();

        topicViews.AddRange(missingDates);

    
        topicViews = topicViews.OrderBy(tv => tv.Date).ToList();

        return topicViews;
    }
}