using System;

public static class DateTimeUtils
{
    public static DateTime FirstDayOfThisWeek(){
        return DateTime.Now.Date.AddDays(-(int) DateTime.Now.Date.DayOfWeek + 1);
    }

    public static DateTime FirstDayOfPreviousWeek(){
        return DateTime.Now.Date.AddDays(-(int)DateTime.Now.Date.DayOfWeek -6);
    }

    public static DateTime FirstDayOfThisMonth(){
        return new DateTime(DateTime.Now.Year,DateTime.Now.Month, 1); 
    }

    public static DateTime FirstDayOfPreviousMonth(){
        return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
    }
}

