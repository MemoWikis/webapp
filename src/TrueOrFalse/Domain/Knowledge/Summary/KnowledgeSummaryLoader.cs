using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;

public class KnowledgeSummaryLoader : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public KnowledgeSummaryLoader(ISession session)
    {
        _session = session;
    }

    public KnowledgeSummary Run(
        int userId, 
        IEnumerable<int> questionIds = null, 
        bool onlyValuated = true)
    {
        var queryOver =
            _session
                .QueryOver<QuestionValuation>()
                .Select(
                    Projections.Group<QuestionValuation>(x => x.KnowledgeStatus),
                    Projections.Count<QuestionValuation>(x => x.KnowledgeStatus)
                )
                .Where(x => x.User.Id == userId);

        if (onlyValuated)
            queryOver.And(x => x.RelevancePersonal != -1);

        if (questionIds != null)
            queryOver.AndRestrictionOn(x => x.Question.Id)
                     .IsIn(questionIds.ToArray());

        var queryResult = queryOver.List<object[]>();

        var result = new KnowledgeSummary();

        foreach (var line in queryResult)
        {
            if ((int) line[0] == (int)KnowledgeStatus.Secure)
                result.Secure = (int)line[1];

            else if ((int) line[0] == (int)KnowledgeStatus.Unknown)
                result.Unknown = (int)line[1];

            else if ((int)line[0] == (int)KnowledgeStatus.Weak)
                result.Weak = (int)line[1];
        }

        return result;
    }
}

