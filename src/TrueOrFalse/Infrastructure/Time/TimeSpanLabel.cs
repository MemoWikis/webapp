
using System;

public class TimeSpanLabel
{
    private bool _useDativ;

    public string Label;
    public int Value;

    public TimeSpanInterval Interval;

    public string Full { get { return Value + " " + Label; } }

    public TimeSpanLabel(TimeSpan timeSpan, bool useDativ = false)
    {
        timeSpan = timeSpan.Duration();
        _useDativ = useDativ;

        var interval = GetBestRemainingInterval(timeSpan);

        switch (interval)
        {
            case TimeSpanInterval.Minutes:
                Value = (int) timeSpan.TotalMinutes;
                Label = Value == 1 ? "Minute" : "Minuten";

                break;

            case TimeSpanInterval.Hours:
                Value = (int)timeSpan.TotalHours;
                Label = Value == 1 ? "Stunde" : "Stunden";
                break;

            case TimeSpanInterval.Days:
                Value = (int)timeSpan.TotalDays;
                Label = Value == 1 ? 
                    "Tag" : 
                    _useDativ ? "Tagen" : "Tage";
                break;

            case TimeSpanInterval.Weeks:
                Value = (int)Math.Round(timeSpan.TotalDays / 7d, 0);
                Label = Value == 1 ? "Woche" : "Wochen";
                break;

            case TimeSpanInterval.Month:
                Value = (int)Math.Round(timeSpan.TotalDays / 30d, 0);
                Label = Value == 1 ? 
                    "Monat" : 
                    _useDativ ? "Monaten" : "Monate";
                break;

            case TimeSpanInterval.Years:
                Value = (int)Math.Round(timeSpan.TotalDays / 365d, 0);
                Label = Value == 1 ? 
                    "Jahr" : 
                    _useDativ ? "Jahren" : "Jahre";
                break;

            default: 
                throw new Exception("unknown interval");
        }
    }

    private TimeSpanInterval GetBestRemainingInterval(TimeSpan timeSpan)
    {
        if (timeSpan.TotalMinutes < 60) return TimeSpanInterval.Minutes;
        if (timeSpan.TotalHours < 24) return TimeSpanInterval.Hours;
        if (timeSpan.TotalDays < 7) return TimeSpanInterval.Days;
        if (timeSpan.TotalDays < 30) return TimeSpanInterval.Weeks;
        if (timeSpan.TotalDays < 356) return TimeSpanInterval.Month;

        return TimeSpanInterval.Years;
    }
}

public enum TimeSpanInterval
{
    Minutes,
    Hours,
    Days,
    Weeks,
    Month,
    Years
}