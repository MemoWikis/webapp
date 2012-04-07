using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace TrueOrFalse.Core
{
    public class UpdateQuestionAnswerCounts : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;

        public UpdateQuestionAnswerCounts(ISession session){
            _session = session;
        }

        public void Run()
        {
            _session.CreateSQLQuery("UPDATE Question SET TotalTrueAnswers = 0 WHERE TotalTrueAnswers is null").ExecuteUpdate();
            _session.CreateSQLQuery("UPDATE Question SET TotalFalseAnswers = 0 WHERE TotalFalseAnswers is null ").ExecuteUpdate();

            _session.CreateSQLQuery(@"UPDATE Question 
                                      SET 
                                       TotalTrueAnswers = agg.CorrectAnswers, 
                                       TotalFalseAnswers = agg.WrongAnswers
                                      FROM Question INNER JOIN(
	                                      SELECT QuestionId, Count(QuestionId) -Sum(Cast(AnswerredCorrectly as Integer)) as WrongAnswers , 
	                                      sum(Cast(AnswerredCorrectly as Integer)) as CorrectAnswers
	                                      FROM AnswerHistory
	                                      GROUP BY QuestionId, AnswerredCorrectly
                                      ) as agg
                                      ON agg.QuestionId = Question.Id").ExecuteUpdate();
        }
    }
}
