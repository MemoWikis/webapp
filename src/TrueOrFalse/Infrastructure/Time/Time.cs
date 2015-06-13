using System;

public struct Time
{
    public int Hours;
    public int Minutes;

    public Time(int hours, int minutes)
    {
        Hours = hours;
        Minutes = minutes;
    }

    public Time(DateTime dateTime)
    {
        Hours = dateTime.Hour;
        Minutes = dateTime.Minute;
    }

    public override string ToString()
    {
        return String.Format("{0:00}:{1:00}", Hours, Minutes);
    }

    public static Time Parse(string time)
    {
        try
        {
            if (String.IsNullOrEmpty(time))
                return new Time(0, 0);

            var parts = time.Split(':', '.');

            if(parts.Length == 0)
                return new Time();

            if(parts.Length == 1)
                return new Time(Convert.ToInt32(parts[0]), 0);

            if(parts.Length == 2)
                return new Time(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]));
        }
        catch{}

        return new Time(0, 0);
    }

    public DateTime SetTime(DateTime dateTime)
    {
        return dateTime.Date + new TimeSpan(Hours, Minutes, 0);
    }
}