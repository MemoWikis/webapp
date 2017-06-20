using NHibernate;
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
}