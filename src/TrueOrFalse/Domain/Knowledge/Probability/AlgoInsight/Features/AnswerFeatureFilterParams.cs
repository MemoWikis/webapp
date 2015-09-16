using System.Collections.Generic;

public class AnswerFeatureFilterParams
{
    public AnswerHistory AnswerHistory;
    public IList<AnswerHistory> PreviousAnswers;
    public Question Question;
    public User User;
}