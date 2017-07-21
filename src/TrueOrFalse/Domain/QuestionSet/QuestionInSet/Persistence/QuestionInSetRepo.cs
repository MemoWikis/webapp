﻿using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using Seedworks.Lib.Persistence;
using Seedworks.Lib.ValueObjects;

public class QuestionInSetRepo : RepositoryDb<QuestionInSet>
{
    public QuestionInSetRepo(ISession session) : base(session){}

    public override void Delete(int questionInSetId)
    {
        var questionInSet = GetById(questionInSetId);
        base.Delete(questionInSetId);

        Sl.R<UpdateSetDataForQuestion>().Run(questionInSet.Question);
    }

    public void DeleteForQuestion(int questionId)
    {
        Session.CreateSQLQuery("DELETE FROM questioninset WHERE Question_id = :questionId")
                .SetParameter("questionId", questionId).ExecuteUpdate();
    }

    public void SaveTimecode(int questionInSetId, string timeCode)
    {
        var questionInSet = GetById(questionInSetId);
        questionInSet.Timecode = Timecode.ToSeconds(timeCode);

        Update(questionInSet);
    }

    public IList<QuestionInSet> GetByQuestionIds(IEnumerable<int> questionIds)
    {
        return Session.QueryOver<QuestionInSet>()
            .Fetch(q => q.Question).Eager
            .Where(Restrictions.In("Id", questionIds.ToArray()))
            .List();
    }
}