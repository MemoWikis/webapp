using System;
using TrueOrFalse;

public static class DateTimeUtils
{
    public static DateTime FirstDayOfThisWeek(){
        return DateTime.Now.Date.AddDays(-(int) DateTime.Now.Date.DayOfWeek + 1);
    }

    public static DateTime FirstDayOfLastWeek(){
        return DateTime.Now.Date.AddDays(-(int)DateTime.Now.Date.DayOfWeek -6);
    }

    public static DateTime FirstDayOfThisMonth(){
        return new DateTime(DateTime.Now.Year,DateTime.Now.Month, 1); 
    }

    public static DateTime FirstDayOfThisYear(){
        return new DateTime(DateTime.Now.Year, 1, 1); 
    }

    public static DateTime FirstDayOfLastYear(){
        return new DateTime(DateTime.Now.Year - 1, 1, 1);
    }

    public static DateTime FirstDayOfLastMonth(){
        return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
    }

    public static string TimeElapsedAsText(DateTime dateTimeBegin)
    {
        return TrueOrFalse.TimeElapsedAsText.Run(dateTimeBegin);
    }
    public static string TimeElapsedAsText(DateTime dateTimeBegin, DateTime dateTimeEnd)
    {
        return TrueOrFalse.TimeElapsedAsText.Run(dateTimeBegin, dateTimeEnd);
    }
}

