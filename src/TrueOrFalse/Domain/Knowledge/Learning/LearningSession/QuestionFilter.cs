
public class QuestionFilterJson
{
    public virtual int MinProbability { get; set; } = 0;
    public virtual int MaxProbability { get; set; } = 100;
    public virtual int MaxQuestionCount { get; set; }
    public virtual bool QuestionsInWishknowledge { get; set; } = false;
    public virtual int QuestionOrder { get; set; }

    public virtual string GetQuestionOrderBy()
    {
        if (QuestionOrder == 0)
            return "NoOrder";
        else if (QuestionOrder == 1)
            return "DescendingProbability";
        else
            return "AscendingProbability";
    }
}
