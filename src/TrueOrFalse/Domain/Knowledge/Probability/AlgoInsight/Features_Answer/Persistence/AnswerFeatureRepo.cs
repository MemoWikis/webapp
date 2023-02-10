using System;
using System.Collections.Generic;
using FluentNHibernate.Utils;
using NHibernate;

public class AnswerFeatureRepo : RepositoryDbBase<AnswerFeature>
{
    public AnswerFeatureRepo(ISession session) : base(session)
    {
    }
    public int GetCount(int featureId)
    {
        return Session.QueryOver<AnswerFeature>()
            .Where(f => f.Id == featureId)
            .JoinQueryOver(f => f.Answers)
            .RowCount();
    }

    public IList<Answer> GetAnswersForFeature(int featureId)
    {
        QuestionFeature questionFeature = null;

        return Session.QueryOver<Answer>()
            .JoinAlias(c => c.Features, () => questionFeature)
            .Where(x => questionFeature.Id == featureId)
            .List();
    }

    public void InsertRelation(int featureId, int answerId)
    {
        _session.CreateSQLQuery(
            String.Format(@"INSERT INTO `answerfeature_to_answer` 
                (`AnswerFeature_id`, `Answer_id`) 
              VALUES 
                ({0}, {1});", featureId, answerId)).ExecuteUpdate();
    }

    public void TruncateTables()
    {
        _session.CreateSQLQuery("truncate table answerfeature").ExecuteUpdate();
        _session.CreateSQLQuery("truncate table answerfeature_to_answer").ExecuteUpdate();
    }
}