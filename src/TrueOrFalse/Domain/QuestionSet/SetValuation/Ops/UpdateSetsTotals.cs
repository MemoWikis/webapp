using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse
{
    public class UpdateSetsTotals : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;
        private readonly CreateOrUpdateSetValue _createOrUpdateSetValue;

        public UpdateSetsTotals(ISession session,CreateOrUpdateSetValue _createOrUpdateSetValue){
            _session = session;
            this._createOrUpdateSetValue = _createOrUpdateSetValue;
        }

        public void UpdateRelevancePersonal(int setId, int userId, int relevance)
        {
            _createOrUpdateSetValue.Run(setId, userId, relevancePeronal: relevance);
            _session.CreateSQLQuery(GenerateRelevancePersonal(setId)).ExecuteUpdate();
            _session.Flush();
        }

        private string GenerateRelevancePersonal(int setId)
        {
            return
                GenerateEntriesQuery("TotalRelevancePersonal", "RelevancePersonal", setId) + " " +
                GenerateAvgQuery("TotalRelevancePersonal", "RelevancePersonal", setId);
        }

        private string GenerateAvgQuery(string fieldToSet, string fieldSource, int setId)
        {
            return "UPDATE QuestionSet SET " + fieldToSet + "Avg = " +
                       "ROUND((SELECT SUM(" + fieldSource + ") FROM SetValuation " +
                       " WHERE SetId = " + setId + " AND " + fieldSource + " != -1)/ " + fieldToSet + "Entries) " +
                   "WHERE Id = " + setId + ";";
        }

        private string GenerateEntriesQuery(string fieldToSet, string fieldSource, int setId)
        {
            return "UPDATE QuestionSet SET " + fieldToSet + "Entries = " +
                       "(SELECT COUNT(Id) FROM SetValuation " +
                       "WHERE SetId = " + setId + " AND " + fieldSource + " != -1) " +
                   "WHERE Id = " + setId + ";";
        }
    }
}
