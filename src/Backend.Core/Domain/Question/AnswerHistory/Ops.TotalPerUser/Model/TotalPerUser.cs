public class TotalPerUser
{
    public int QuestionId;
    public int TotalTrue;
    public int TotalFalse;

    public int Total() => TotalTrue + TotalFalse;
}