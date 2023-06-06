public class TimeSpanLabel
{
    private readonly bool _showTimeUnit;
    public string Label;
    public int Value;
    public bool TimeSpanIsNegative;

    public TimeSpanInterval Interval;

    public string Full => _showTimeUnit 
        ? Value + Label
        : Value + " " + Label;

    public TimeSpanLabel(TimeSpan timeSpan, bool useDativ = false, bool showTimeUnit = false)
    {
        _showTimeUnit = showTimeUnit;

        TimeSpanIsNegative = timeSpan.Ticks < 0;

        timeSpan = timeSpan.Duration();

        var interval = GetBestRemainingInterval(timeSpan);

        switch (interval)
        {
            case TimeSpanInterval.Minutes:
                Value = (int) timeSpan.TotalMinutes;

                Label = showTimeUnit 
                    ? "min" 
                    : Value == 1  ? "Minute" : "Minuten";
                break;

            case TimeSpanInterval.Hours:
                Value = (int)timeSpan.TotalHours;
                Label = showTimeUnit
                    ? "h"
                    : Value == 1 ? "Stunde" : "Stunden";
                break;

            case TimeSpanInterval.Days:
                Value = (int)timeSpan.TotalDays;
                Label = showTimeUnit
                    ? "t"
                    : Value == 1 
                        ? "Tag" 
                        : useDativ ? "Tagen" : "Tage";
                break;

            case TimeSpanInterval.Weeks:
                Value = (int)Math.Round(timeSpan.TotalDays / 7d, 0);
                Label = showTimeUnit
                    ? "w" 
                    : Value == 1  ? "Woche"  : "Wochen";
                break;

            case TimeSpanInterval.Month:
                Value = (int)Math.Round(timeSpan.TotalDays / 30d, 0);
                Label = Value == 1 ? 
                    "Monat" :
                    useDativ ? "Monaten" : "Monate";
                break;

            case TimeSpanInterval.Years:
                Value = (int)Math.Round(timeSpan.TotalDays / 365d, 0);
                Label = Value == 1 ? 
                    "Jahr" :
                    useDativ ? "Jahren" : "Jahre";
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