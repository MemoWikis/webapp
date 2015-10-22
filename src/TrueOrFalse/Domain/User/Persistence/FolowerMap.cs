
using FluentNHibernate.Mapping;

public class FollowerMap : ClassMap<FollowerInfo>
{
    public FollowerMap()
    {
        Table("user_to_follower");

        Id(x => x.Id);

        References(x => x.Follower).Column("Follower_id");
        References(x => x.User).Column("User_id");

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}
