
public class QuestionFilterJson
{
    public virtual string Modus { get; set; }
    public virtual int MinProbability { get; set; }
    public virtual int MaxProbability { get; set; }
    public virtual int MaxQuestionCount { get; set; }
    public virtual bool QuestionsInWishknowledge { get; set; }
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
