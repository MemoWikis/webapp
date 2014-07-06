using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrueOrFalse
{
    public class AnswerHistoryModel 
    {
        public int TimesAnsweredTotal;
        public int TimesAnsweredUserTrue;
        public int TimesAnsweredUserWrong;
        public int TimesAnsweredUser;
        public int TimesAnsweredWrongTotal;
        public int TimesAnsweredCorrect;

        public AnswerHistoryModel(Question question, TotalPerUser valuationForUser)
        {
            valuationForUser = valuationForUser ?? new TotalPerUser();

            TimesAnsweredTotal = question.TotalAnswers();
            TimesAnsweredCorrect = question.TotalTrueAnswers;
            TimesAnsweredWrongTotal = question.TotalFalseAnswers;

            TimesAnsweredUser = valuationForUser.Total();
            TimesAnsweredUserTrue = valuationForUser.TotalTrue;
            TimesAnsweredUserWrong = valuationForUser.TotalFalse;
        }
    }
}
