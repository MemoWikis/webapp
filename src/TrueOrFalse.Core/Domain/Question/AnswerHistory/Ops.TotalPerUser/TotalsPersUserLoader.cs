using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Transform;

namespace TrueOrFalse.Core
{
    public class TotalsPersUserLoader : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;

        public TotalsPersUserLoader(ISession session){
            _session = session;
        }

        public IList<TotalPerUser> Run(int userId, IEnumerable<int> questionIds)
        {
            if (!questionIds.Any())
                throw new Exception("at least one question Id is expected");

            var sbQuestionIdRestriction = new StringBuilder();
            foreach (var questionId in questionIds)
                sbQuestionIdRestriction.AppendLine("AND QuestionId = " + questionId);

            var query = String.Format(
                @"SELECT 
	                  QuestionId, 
	                  COUNT(QuestionId) -SUM(CAST(AnswerredCorrectly as Integer)) as WrongAnswers , 
	                  SUM(CAST(AnswerredCorrectly as Integer)) as CorrectAnswers
                  FROM AnswerHistory
                  GROUP BY QuestionId, UserId
                  HAVING UserId = {0} 
                  {1}", userId, sbQuestionIdRestriction);

            return _session.CreateSQLQuery(query).
                            SetResultTransformer(Transformers.AliasToBean(typeof(TotalPerUser))).
                            List<TotalPerUser>();
        }
    }
}
