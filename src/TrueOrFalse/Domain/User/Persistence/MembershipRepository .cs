using NHibernate;

public class MembershipRepo : RepositoryDbBase<Membership>
{
    public MembershipRepo(ISession session): base(session){
    }
}