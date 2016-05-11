using NHibernate;

public class LearningSessionStepRepo : RepositoryDbBase<LearningSessionStep>
{
    public LearningSessionStepRepo(ISession session): base(session){

    }

    public void DeleteForQuestion(int questionId)
    {
        Session.CreateSQLQuery("DELETE FROM learningsessionstep WHERE Question_id = :questionId")
                .SetParameter("questionId", questionId).ExecuteUpdate();
    }

}