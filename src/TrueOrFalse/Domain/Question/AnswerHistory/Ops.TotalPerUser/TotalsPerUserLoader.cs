﻿using NHibernate;
using System.Text;

public class TotalsPerUserLoader(ISession session) : IRegisterAsInstancePerLifetime
{
    public TotalPerUser Run(int userId, int questionId)
    {
        var result = Run(userId, new List<int> { questionId });
        if (!result.Any())
            return new TotalPerUser();

        return result[0];
    }

    public IList<TotalPerUser> Run(int userId, IEnumerable<int> questionIds)
    {
        if (!questionIds.Any())
            return new TotalPerUser[] { };

        var totals = new List<TotalPerUser>();

        if (userId == 0)
        {
            var questions = EntityCache.GetQuestionsByIds(questionIds);

            foreach (var question in questions)
            {
                var answers = question.AnswersByAnonymousUsers;
                var totalTrue = answers.Count(a => a.AnswerredCorrectly == AnswerCorrectness.True || a.AnswerredCorrectly == AnswerCorrectness.MarkedAsTrue);
                var totalFalse = answers.Count(a => a.AnswerredCorrectly == AnswerCorrectness.False);

                totals.Add(new TotalPerUser
                {
                    QuestionId = question.Id,
                    TotalTrue = totalTrue,
                    TotalFalse = totalFalse
                });
            }
        }
        else if (userId > -1)
        {
            var sbQuestionIdRestriction = new StringBuilder();

            var firstHit = true;
            foreach (var questionId in questionIds)
                if (firstHit)
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

            totals = session.CreateSQLQuery(query)
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
            if (totals.All(t => t.QuestionId != questionId))
                totals.Add(new TotalPerUser
                {
                    QuestionId = questionId,
                });

        return totals;
    }
}