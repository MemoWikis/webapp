using System.Text;
using NHibernate;

namespace TrueOrFalse
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

            sb.Append(GenerateEntriesQuery("TotalRelevancePersonal", "RelevancePersonal", questionValuation.QuestionId));
            sb.Append(GenerateAvgQuery("TotalRelevancePersonal", "RelevancePersonal", questionValuation.QuestionId));    
            
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
                GenerateEntriesQuery("TotalQuality", "Quality", questionId) + " " +
                GenerateAvgQuery("TotalQuality", "Quality", questionId) ;
        }

        private string GenerateRelevancePersonal(int questionId)
        {
            return
                GenerateEntriesQuery("TotalRelevancePersonal", "RelevancePersonal", questionId) + " " +
                GenerateAvgQuery("TotalRelevancePersonal", "RelevancePersonal", questionId);
        }

        private string GenerateRelevanceAllQuery(int questionId)
        {
            return
                GenerateEntriesQuery("TotalRelevanceForAll", "RelevanceForAll", questionId) + " " +
                GenerateAvgQuery("TotalRelevanceForAll", "RelevanceForAll", questionId);
        }

        private string GenerateAvgQuery(string fieldToSet, string fieldSource, int questionId)
        {
            return "UPDATE Question SET " + fieldToSet + "Avg = " +
                       "ROUND((SELECT SUM(" + fieldSource + ") FROM QuestionValuation " +
                       " WHERE QuestionId = " + questionId + " AND " + fieldSource + " != -1)/ " + fieldToSet + "Entries) " +
                   "WHERE Id = " + questionId + ";";
        }

        private string GenerateEntriesQuery(string fieldToSet, string fieldSource, int questionId)
        {
            return "UPDATE Question SET " + fieldToSet + "Entries = " +
                       "(SELECT COUNT(Id) FROM QuestionValuation " +
                       "WHERE QuestionId = " + questionId + " AND " + fieldSource + " != -1) " +
                   "WHERE Id = " + questionId + ";";
        }
    }
}