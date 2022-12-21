using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

public class TotalsPersUserLoader : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public TotalsPersUserLoader(ISession session){
        _session = session;
    }

    public TotalPerUser Run(int userId, int questionId)
    {
        var result = Run(userId, new List<int> {questionId});
        if(!result.Any())
            return new TotalPerUser();

        return result[0];
    }

    public IList<TotalPerUser> Run(int userId, IList<QuestionCacheItem> questions)
    {
        return Run(userId, questions.GetIds());
    }

    public IList<TotalPerUser> Run(int userId, IEnumerable<int> questionIds)
    {
        if (!questionIds.Any())
            return new TotalPerUser[]{};


        var totals = new List<TotalPerUser>();

        if(userId > -1)
        { 
            var sbQuestionIdRestriction = new StringBuilder();

            var firstHit = true;
            foreach (var questionId in questionIds)
                if(firstHit)
                {
                    sbQuestionIdRestriction.AppendLine("AND QuestionId = " + questionId);
                    firstHit = false;
                }
                else
                    sbQuestionIdRestriction.AppendLine("OR QuestionId = " + questionId);
            
            var query = String.Format(
                @"SELECT 
	                    QuestionId,
	                    CAST(SUM(CASE WHEN AnswerredCorrectly = 1 THEN 1 WHEN AnswerredCorrectly = 2 THEN 1 ELSE 0 END) AS signed INTEGER) as TotalTrue,
	                    CAST(SUM(CASE WHEN AnswerredCorrectly = 0 THEN 1 ELSE 0 END) AS signed INTEGER) as TotalFalse
                    FROM Answer
                    WHERE UserId = {0}
                    GROUP BY QuestionId, UserId
                    HAVING UserId = {0} 
                    {1}", userId, sbQuestionIdRestriction);

            totals = _session.CreateSQLQuery(query)
                            .List<object>()
                            .Select(item => new TotalPerUser
                                {
                                    QuestionId = Convert.ToInt32(((object[])item)[0]),
                                    TotalTrue = Convert.ToInt32(((object[])item)[1]),
                                    TotalFalse = Convert.ToInt32(((object[])item)[2]),
                                })
                            .ToList();

        }


        foreach (var questionId in questionIds)
            if(totals.All(t => t.QuestionId != questionId))
                totals.Add(new TotalPerUser
                {
                    QuestionId = questionId,
                });

        return totals;
    }
}