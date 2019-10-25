
public class QuestionFilterJson
{
    public int MinProbability { get; set; } = 0;
    public int MaxProbability { get; set; } = 100;
    public int MaxQuestionCount { get; set; }
    public bool QuestionsInWishknowledge { get; set; } = false;
    public int QuestionOrder { get; set; }

    public string GetQuestionOrderBy()
    {
        if (QuestionOrder == 0)
            return "NoOrder";
        else if (QuestionOrder == 1)
            return "DescendingProbability";
        else
            return "AscendingProbability";
    }
}
