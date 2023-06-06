public class Period
{
    public Period(Time start, Time end)
    {
        Start = start;
        End = end;
    }

    public Time Start;
    public Time End;


    public bool IsInPeriod(DateTime dateTime)
    {
        return IsInPeriod(Time.New(dateTime));
    }

    public bool IsInPeriod(Time time)
    {
        if(Start < End)
            if (time >= Start && time <= End)
                return true;

        if (Start > End)
            if ((time >= Start && time <= Time.New(24, 00)) || (time >= Time.New(00, 00)  && time <= End))
                return true;

        return false;
    }
}