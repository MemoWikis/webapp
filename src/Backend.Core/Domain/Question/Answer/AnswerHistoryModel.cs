public class AnswerHistoryModel 
{
    public int TimesAnsweredTotal;
    public int TimesAnsweredUserTrue;
    public int TimesAnsweredUserWrong;
    public int TimesAnsweredUser;
    public int TimesAnsweredWrongTotal;
    public int TimesAnsweredCorrect;

    public AnswerHistoryModel(QuestionCacheItem question, TotalPerUser valuationForUser)
    {
        TimesAnsweredTotal = question.TotalAnswers();
        TimesAnsweredCorrect = question.TotalTrueAnswers;
        TimesAnsweredWrongTotal = question.TotalFalseAnswers;

        TimesAnsweredUser = valuationForUser.Total();
        TimesAnsweredUserTrue = valuationForUser.TotalTrue;
        TimesAnsweredUserWrong = valuationForUser.TotalFalse;
    }
}