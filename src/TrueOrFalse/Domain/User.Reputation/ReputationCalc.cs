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

            var wishCountQuestions = _session.CreateSQLQuery(query)
                .List<object>()
                .Select(item => new Tuple<int, int>(
                    Convert.ToInt32(((object[])item)[0]),
                    Convert.ToInt32(((object[])item)[1]))
                )
                .ToList();

            result.ForQuestionsWishCount = wishCountQuestions.Sum(q => q.Item1) * 10;
            result.ForQuestionsWishKnow = wishCountQuestions.Sum(q => q.Item2);


            query =
                String.Format(
@"SELECT count(sv.SetId), sum(sv.RelevancePersonal)
FROM setvaluation sv
LEFT JOIN questionset s
ON sv.SetId = s.Id
WHERE s.Creator_id = {0}
AND sv.UserId <> {0}
AND sv.RelevancePersonal <> -1
GROUP BY 
	sv.SetId, 
	sv.RelevancePersonal", user.Id);

            var wishCountSets = _session.CreateSQLQuery(query)
                .List<object>()
                .Select(item => new Tuple<int, int>(
                    Convert.ToInt32(((object[])item)[0]),
                    Convert.ToInt32(((object[])item)[1]))
                )
                .ToList();

            result.ForSetWishCount = wishCountSets.Sum(s => s.Item1) * 10;
            result.ForSetWishKnow = wishCountSets.Sum(s => s.Item2);
            
            return result;
        }
    }
}
