using System;
using NHibernate;
using NHibernate.Transform;

public class GetSetTotal : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public GetSetTotal(ISession session){
        _session = session;
    }

    public GetSetTotalResult RunForRelevancePersonal(int setId)
    {
        return _session.CreateSQLQuery(GetQuery("TotalRelevancePersonalEntries", "TotalRelevancePersonalAvg", setId))
                        .SetResultTransformer(Transformers.AliasToBean(typeof(GetSetTotalResult)))
                        .UniqueResult<GetSetTotalResult>();
    }

    private string GetQuery(string entriesField, string avgField, int questionId)
    {
        return String.Format("SELECT {0} as Count, {1} as Avg FROM QuestionSet WHERE Id = {2}", 
                                entriesField, avgField, questionId);
    }
}