using System;

public class AboutMemuchoModel : BaseModel
{
    public int TotalActivityPoints;
    public int TotalQuestionCount;
    //public int TotalQuestionCountRound100;
    public int PercentageQuestionsAnsweredMostlyWrong;

    public AboutMemuchoModel()
    {
        TotalActivityPoints = Sl.R<UserRepo>().GetTotalActivityPoints();
        TotalQuestionCount = Sl.R<QuestionRepo>().TotalPublicQuestionCount();
        //TotalQuestionCountRound100 = (int) Math.Floor(TotalQuestionCount / 100.0) * 100;
        PercentageQuestionsAnsweredMostlyWrong = Sl.R<QuestionRepo>().GetPercentageQuestionsAnsweredMostlyWrong();
    }
}