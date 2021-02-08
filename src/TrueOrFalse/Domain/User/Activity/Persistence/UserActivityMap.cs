using FluentNHibernate.Mapping;

public class UserActivityMap : ClassMap<UserActivity>
{
    public UserActivityMap()
    {
        Id(x => x.Id);

        References(x => x.UserConcerned);
        Map(x => x.At);
        Map(x => x.Type);
        References(x => x.Question);
        References(x => x.Category);
        References(x => x.UserIsFollowed);
        References(x => x.UserCauser);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }           
}
