using System.Collections.Generic;

public class AnswerFeatureFilterParams
{
    public Answer Answer;
    public IList<Answer> PreviousAnswers;
    public Question Question;
    public User User;

    /// <summary>
    /// Answer + PreviousAnswer
    /// </summary>
    public IList<Answer> Answers()
    {   
        var result = new List<Answer>();
        result.AddRange(PreviousAnswers);
        result.Add(Answer);
        return result;
    }
}