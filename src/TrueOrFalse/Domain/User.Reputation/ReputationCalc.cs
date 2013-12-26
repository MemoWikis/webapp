using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace TrueOrFalse
{
    public class ReputationCalc : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;

        public ReputationCalc(ISession session){
            _session = session;
        }

        public ReputationCalcResult Run(User user)
        {
            var result = new ReputationCalcResult();

            var createdQuetions = _session.QueryOver<Question>()
                .Where(q => q.Creator.Id == user.Id).List<Question>();

            result.ForQuestionsCreated = createdQuetions.Count * 5;

            string query = 
                String.Format(
@"SELECT count(qv.QuestionId), sum(qv.RelevancePersonal)
FROM questionvaluation qv
LEFT JOIN question q
ON qv.QuestionId = q.Id
WHERE q.Creator_id = {0}
AND qv.UserId <> {0}
AND qv.RelevancePersonal <> -1
GROUP BY 
	qv.QuestionId, 
	qv.RelevancePersonal", user.Id);

            var wishCount = _session.CreateSQLQuery(query)
                .List<object>()
                .Select(item => new Tuple<int, int>(
                    Convert.ToInt32(((object[])item)[0]),
                    Convert.ToInt32(((object[])item)[1]))
                )
                .ToList();

            result.ForQuestionsWishCount = wishCount.Sum(x => x.Item1) * 10;
            result.ForQuestionsWishKnow = wishCount.Sum(x => x.Item2);
            
            return result;
        }
    }
}
