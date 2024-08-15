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

    public static DateTime RoundUp(DateTime dateTime, TimeSpan roundToNextFull)
    {
        //http://stackoverflow.com/a/7029464

        return new DateTime(((dateTime.Ticks + roundToNextFull.Ticks - 1) / roundToNextFull.Ticks) *
                            roundToNextFull.Ticks);
    }

    public static bool IsToday(CategoryCacheItem topic)
    {
        return IsToday(topic.DateCreated); 
    }

    public static bool IsLastLoginToday(UserCacheItem user)
    {
        if (user.LastLogin == null)
        {
            return false;
        }
        return IsToday(user.LastLogin.Value);
    }

    public static bool IsRegisterToday(UserCacheItem user)
    {
        return IsToday(user.DateCreated);
    }

    private static bool IsToday(DateTime date)
    {
        return date.Date == DateTime.Now.Date;
    }
}