using NHibernate;

public class LearningSessionRepo : RepositoryDbBase<Membership>
{
    public LearningSessionRepo(ISession session): base(session){

    }
}