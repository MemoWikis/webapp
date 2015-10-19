using NHibernate;

public class LearningSessionRepo : RepositoryDbBase<LearningSession>
{
    public LearningSessionRepo(ISession session): base(session){

    }
}