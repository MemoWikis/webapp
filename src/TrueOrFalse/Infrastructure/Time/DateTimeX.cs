public class DateTimeX
{
    public static int OffsetInSec { get; private set; }

    public static DateTime Now()
    {
        if (OffsetInSec != 0)
            return DateTime.Now.AddSeconds(OffsetInSec);

        return DateTime.Now;
    }

    public static void ResetOffset()
    {
        OffsetInSec = 0;
    }
}