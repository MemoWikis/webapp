public class Period
{
    public Period(Time start, Time end)
    {
        Start = start;
        End = end;
    }

    public Time Start;
    public Time End;

    public bool IsInPeriod(Time time)
    {
        return false;
    }
}