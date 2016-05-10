﻿using NHibernate;
using Seedworks.Lib.Persistence;

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

}