using System.Text.RegularExpressions;
using static System.String;

public class Timecode
{
    public static int ToSeconds(string timecode)
    {
        if (timecode == null)
            return 0;

        int seconds = 0;
        int minutes = 0;

        timecode = new Regex(@"[^\d|:]").Replace(timecode, "");

        if (IsNullOrEmpty(timecode))
            return 0;

        if (timecode.Contains(":"))
        {
            var timecodeParts = timecode.Split(':');

            seconds = Convert.ToInt32(timecodeParts[1]);
            minutes = Convert.ToInt32(timecodeParts[0]);
        }
        else
        {
            seconds = Convert.ToInt32(timecode);
        }

        return minutes * 60 + seconds;
    }

    public static string ToString(int seconds)
    {
        var duration = new TimeSpan(0, 0, seconds);

        return $"{duration.Minutes:D1}:{duration.Seconds:D2}";
    }
}