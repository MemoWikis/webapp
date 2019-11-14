
public class QuestionFilterJson
{
    public int MinProbability { get; set; } = 0;
    public int MaxProbability { get; set; } = 100;
    public int MaxQuestionCount { get; set; } = 0;
    public bool QuestionsInWishknowledge { get; set; } = false;
    public int QuestionOrder { get; set; }

    public string GetQuestionOrderBy()
    {
        var orderText = "NoOrder";
        if (QuestionOrder == 1)
            orderText = "HighProbability";
        else if (QuestionOrder == 2)
            orderText = "LowProbability";

        return orderText;
    }

    public bool HasMaxQuestionCount()
    {
        if (MaxQuestionCount > 0)
            return true;

        return false;
    }
}
