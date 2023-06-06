using System.Linq;
using NHibernate;
using NHibernate.Transform;

public class GetStreakCorrectness
{
    public static GetStreakCorrectnessResult Run(int userId)
    {
        var query = @"
            SELECT 
	            MIN(t.DateCreated) as StartDate,
	            MAX(t.DateCreated) as EndDate,
	            CAST(COUNT(*) as signed INTEGER) as StreakLength 
            FROM
	            (SELECT 
		            CASE 
			            WHEN 
				            ah_outer.AnswerredCorrectly = 1 OR
				            ah_outer.AnswerredCorrectly = 2
			            THEN 1
			            ELSE 0
		            END as Correctly,
		            ah_outer.DateCreated, 
		            (
			            SELECT COUNT(*)
			            FROM Answer ah_inner
			            WHERE
				            CASE 
					            WHEN 
						            ah_inner.AnswerredCorrectly = 1 OR
						            ah_inner.AnswerredCorrectly = 2
					            THEN 1
					            ELSE 0
				            END != ah_outer.AnswerredCorrectly
			            AND ah_inner.DateCreated < ah_outer.DateCreated
		            ) as RunGroup
	            FROM Answer ah_outer
	            WHERE UserId = {0}
	            AND (
		            AnswerredCorrectly = 1 OR 
		            AnswerredCorrectly = 2
	            ) )as t
            GROUP BY RunGroup";

        var streaks = 
            Sl.R<ISession>()
                .CreateSQLQuery(String.Format(query, userId))
                .SetResultTransformer(Transformers.AliasToBean(typeof(GetStreakCorrectnessResult)))
                .List<GetStreakCorrectnessResult>();

        if (streaks == null)
            return null;

        return streaks.OrderByDescending(x => x.StreakLength).First();
    }
}