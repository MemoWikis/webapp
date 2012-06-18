using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace TrueOrFalse.Core
{
    public class UpdateQuestionTotals : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionValuationRepository _questionValuationRepository;
        private readonly ISession _session;

        public UpdateQuestionTotals(QuestionValuationRepository questionValuationRepository, 
                                    ISession session)
        {
            _questionValuationRepository = questionValuationRepository;
            _session = session;
        }

        public void Run(QuestionValuation questionValuation)
        {
            _questionValuationRepository.CreateOrUpdate(questionValuation);

            var sb = new StringBuilder();

            sb.Append(GenerateAvgQuery("TotalQualityAvg", "Quality", questionValuation.QuestionId, questionValuation.UserId));
            sb.Append(GenerateAvgQuery("TotalRelevanceForAllAvg", "RelevanceForAll", questionValuation.QuestionId, questionValuation.UserId));
            sb.Append(GenerateAvgQuery("TotalRelevancePersonalAvg", "RelevancePersonal", questionValuation.QuestionId, questionValuation.UserId));

            sb.Append(GenerateEntriesQuery("TotalQualityEntries", "Quality", questionValuation.QuestionId, questionValuation.UserId));
            sb.Append(GenerateEntriesQuery("TotalRelevanceForAllEntries", "RelevanceForAll", questionValuation.QuestionId, questionValuation.UserId));
            sb.Append(GenerateEntriesQuery("TotalRelevancePersonalEntries", "RelevancePersonal", questionValuation.QuestionId, questionValuation.UserId));

            _session.CreateSQLQuery(sb.ToString()).ExecuteUpdate();
            _session.Flush();
        }

        public string GenerateAvgQuery(string fieldToSet, string fieldSource, int questionId, int userId)
        {
            return String.Format("UPDATE Question SET {0} = " +
                                   "(SELECT SUM({1}) FROM QuestionValuation " +
	                               "WHERE UserId = " + userId + " AND QuestionId = " + questionId + " AND {2} != -1) " +
                                 "WHERE Id = "+ questionId + ";", fieldToSet, fieldSource, fieldSource);
        }

        public string GenerateEntriesQuery(string fieldToSet, string fieldSource, int questionId, int userId)
        {
            return String.Format("UPDATE Question SET {0} = " +
                                   "(SELECT COUNT(Id) FROM QuestionValuation " +
                                   "WHERE UserId = " + userId + " AND QuestionId = " + questionId + " AND {2} != -1) " +
                                 "WHERE Id = "+ questionId + ";", fieldToSet, fieldSource, fieldSource);
        }
    }
}
