using System.Collections.Generic;

public class AnswerFeatureFilterParams
{
    public Answer Answer;
    public IList<Answer> PreviousAnswers;
    public Question Question;
    public User User;
}