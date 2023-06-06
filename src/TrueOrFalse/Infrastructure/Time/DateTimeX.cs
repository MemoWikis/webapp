public class DateTimeX
{
    public static int OffsetInSec { get; private set; }

    public static DateTime Now()
    {
        if (OffsetInSec != 0)
            return DateTime.Now.AddSeconds(OffsetInSec);

        return DateTime.Now;
    }

    public static void Forward(int secs = 0, int mins = 0, int hours = 0, int days = 0)
    {
        if (secs != 0) OffsetInSec += secs;
        if (mins != 0) OffsetInSec += mins * 60;
        if (hours != 0) OffsetInSec += hours * 60 * 60;
        if (days != 0) OffsetInSec += days * 60 * 60 * 24;
    }

    public static void ResetOffset()
    {
        OffsetInSec = 0;
    }
}