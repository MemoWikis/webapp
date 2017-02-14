public class SetActiveMemory
{
    public int TotalQuestions;
    public int TotalInActiveMemory;

    public int TotalNotInMemory => TotalQuestions - TotalInActiveMemory;
}