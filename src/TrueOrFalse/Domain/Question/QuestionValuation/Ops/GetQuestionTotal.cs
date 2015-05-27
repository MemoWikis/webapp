using System;
using NHibernate;
using NHibernate.Transform;

public class GetQuestionTotal : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public GetQuestionTotal(ISession session)
    {
        _session = session;
    }

    public GetQuestionTotalResult RunForQuality(int questionId)
    {
        return _session.CreateSQLQuery(GetQuery("TotalQualityEntries", "TotalQualityAvg", questionId))
                        .SetResultTransformer(Transformers.AliasToBean(typeof(GetQuestionTotalResult)))
                        .UniqueResult<GetQuestionTotalResult>();
    }

    public GetQuestionTotalResult RunForRelevancePersonal(int questionId)
    {
        return _session.CreateSQLQuery(GetQuery("TotalRelevancePersonalEntries", "TotalRelevancePersonalAvg", questionId))
                        .SetResultTransformer(Transformers.AliasToBean(typeof(GetQuestionTotalResult)))
                        .UniqueResult<GetQuestionTotalResult>();
    }

    public GetQuestionTotalResult RunForRelevanceForAll(int questionId)
    {
        return _session.CreateSQLQuery(GetQuery("TotalRelevanceForAllEntries", "TotalRelevanceForAllAvg", questionId))
                        .SetResultTransformer(Transformers.AliasToBean(typeof(GetQuestionTotalResult)))
                        .UniqueResult<GetQuestionTotalResult>();
    }

    private string GetQuery(string entriesField, string avgField, int questionId)
    {
        return String.Format("SELECT {0} as Count, {1} as Avg FROM Question WHERE Id = {2}", 
                                entriesField, avgField, questionId);
    }
}