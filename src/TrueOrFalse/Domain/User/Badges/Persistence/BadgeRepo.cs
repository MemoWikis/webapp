using NHibernate;

public class BadgeRepo : RepositoryDbBase<Badge>
{
    public BadgeRepo(ISession session) : base(session)
    {
    }

    public bool Exists(string badgeKey, int userId)
    {
        return
            _session.QueryOver<Badge>()
                .Where(b =>
                    b.BadgeTypeKey == badgeKey &&
                    b.User.Id == userId)
                .SingleOrDefault() != null;
    }
}