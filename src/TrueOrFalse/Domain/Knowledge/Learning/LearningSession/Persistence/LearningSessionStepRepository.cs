using NHibernate;

public class LearningSessionStepRepo : RepositoryDbBase<LearningSessionStep>
{
    public LearningSessionStepRepo(ISession session): base(session){

    }
}