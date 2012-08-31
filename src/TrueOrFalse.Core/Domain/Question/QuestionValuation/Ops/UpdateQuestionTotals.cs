using System;
using System.Text;
using NHibernate;

namespace TrueOrFalse.Core
{
    public class UpdateQuestionTotals : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionValuationRepository _questionValuationRepository;
        private readonly CreateOrUpdateQuestionValue _createOrUpdateQuestionValue;
        private readonly ISession _session;

        public UpdateQuestionTotals(QuestionValuationRepository questionValuationRepository,
                                    CreateOrUpdateQuestionValue createOrUpdateQuestionValue,
                                    ISession session)
        {
            _questionValuationRepository = questionValuationRepository;
            _createOrUpdateQuestionValue = createOrUpdateQuestionValue;
            _session = session;
        }

        public void Run(QuestionValuation questionValuation)
        {
            _questionValuationRepository.CreateOrUpdate(questionValuation);

            var sb = new StringBuilder();

            sb.Append(GenerateQualityQuery(questionValuation.QuestionId));
            sb.Append(GenerateRelevanceAllQuery(questionValuation.QuestionId));
            
            sb.Append(GenerateAvgQuery("TotalRelevancePersonalAvg", "RelevancePersonal", questionValuation.QuestionId));    
            sb.Append(GenerateEntriesQuery("TotalRelevancePersonalEntries", "RelevancePersonal", questionValuation.QuestionId));

            _session.CreateSQLQuery(sb.ToString()).ExecuteUpdate();
            _session.Flush();
        }
        
        public void UpdateQuality(int questionId, int userId, int quality)
        {
            _createOrUpdateQuestionValue.Run(questionId, userId, quality: quality);
            _session.CreateSQLQuery(GenerateQualityQuery(questionId)).ExecuteUpdate();
            _session.Flush();
        }

        public void UpdateRelevancePersonal(int questionId, int userId, int relevance)
        {
            _createOrUpdateQuestionValue.Run(questionId, userId, relevancePeronal: relevance);
            _session.CreateSQLQuery(GenerateRelevancePersonal(questionId)).ExecuteUpdate();
            _session.Flush();            
        }

        public void UpdateRelevanceAll(int questionId, int userId, int relevance)
        {
            _createOrUpdateQuestionValue.Run(questionId, userId, relevanceForAll: relevance);
            _session.CreateSQLQuery(GenerateRelevanceAllQuery(questionId)).ExecuteUpdate();
            _session.Flush();            
        }

        private string GenerateQualityQuery(int questionId)
        {
            return 
             GenerateAvgQuery("TotalQualityAvg", "Quality", questionId) + " " +
             GenerateEntriesQuery("TotalQualityEntries", "Quality", questionId);
        }

        private string GenerateRelevancePersonal(int questionId)
        {
            return
                GenerateAvgQuery("TotalRelevancePersonalAvg", "RelevancePersonal", questionId) + " " +
                GenerateEntriesQuery("TotalRelevancePersonalEntries", "RelevancePersonal", questionId);
        }

        private string GenerateRelevanceAllQuery(int questionId)
        {
            return 
                GenerateAvgQuery("TotalRelevanceForAllAvg", "RelevanceForAll", questionId) + " " +
                GenerateEntriesQuery("TotalRelevanceForAllEntries", "RelevanceForAll", questionId);
        }

        private string GenerateAvgQuery(string fieldToSet, string fieldSource, int questionId)
        {
            return String.Format("UPDATE Question SET {0} = " +
                                   "(SELECT SUM({1}) FROM QuestionValuation " +
	                               "WHERE QuestionId = " + questionId + " AND {2} != -1) " +
                                 "WHERE Id = "+ questionId + ";", fieldToSet, fieldSource, fieldSource);
        }

        private string GenerateEntriesQuery(string fieldToSet, string fieldSource, int questionId)
        {
            return String.Format("UPDATE Question SET {0} = " +
                                   "(SELECT COUNT(Id) FROM QuestionValuation " +
                                   "WHERE QuestionId = " + questionId + " AND {2} != -1) " +
                                 "WHERE Id = "+ questionId + ";", fieldToSet, fieldSource, fieldSource);
        }

    }
}
