using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;
using TrueOrFalse;

public class KnowledgeSummaryLoader : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public KnowledgeSummaryLoader(ISession session)
    {
        _session = session;
    }

    public KnowledgeSummary Run(int userId)
    {
        var queryResult = 
            _session
                .QueryOver<QuestionValuation>()
                .Select(
                    Projections.Group<QuestionValuation>(x => x.KnowledgeStatus),
                    Projections.Count<QuestionValuation>(x => x.KnowledgeStatus)
                )
                .Where(x => 
                    x.UserId == userId && 
                    x.RelevancePersonal != -1
                ).List<object[]>();


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

