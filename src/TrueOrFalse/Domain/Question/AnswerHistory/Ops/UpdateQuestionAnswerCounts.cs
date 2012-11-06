using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace TrueOrFalse
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

            _session.CreateSQLQuery(@"UPDATE 
	                                      Question q,
	                                      ( SELECT agg.CorrectAnswers, agg.WrongAnswers, q.Id as aggQuestionId
	                                      FROM Question AS q INNER JOIN(
	                                        SELECT QuestionId, COUNT(QuestionId) -SUM(AnswerredCorrectly) AS WrongAnswers,
	                                        SUM(AnswerredCorrectly) as CorrectAnswers
	                                        FROM AnswerHistory
	                                        GROUP BY QuestionId, AnswerredCorrectly
	                                      ) AS agg ) as agg
                                      SET 
	                                      TotalTrueAnswers = agg.CorrectAnswers, 
	                                      TotalFalseAnswers = agg.WrongAnswers
                                      WHERE q.Id = aggQuestionId").ExecuteUpdate();
        }
    }
}
