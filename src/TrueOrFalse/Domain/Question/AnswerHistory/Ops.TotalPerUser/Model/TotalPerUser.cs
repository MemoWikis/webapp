public class TotalPerUser
{
    public int QuestionId;
    public int TotalTrue;
    public int TotalFalse;

    public int Total(){ return TotalTrue + TotalFalse;}
    public int PercentageTrue()
    {
        if (Total() == 0) return 0;
        if (TotalTrue== 0) return 0;
        return Convert.ToInt32(((decimal)TotalTrue/ Total()) * 100);
    }

    public int PercentageFalse()
    {
        if (Total() == 0) return 0;
        if (TotalFalse == 0) return 0;
        return Convert.ToInt32(((decimal)TotalFalse / Total()) * 100);
    }
}