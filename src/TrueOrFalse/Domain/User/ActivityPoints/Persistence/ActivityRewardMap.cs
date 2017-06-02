using FluentNHibernate.Mapping;

public class ActivityRewardMap : ClassMap<ActivityReward>
{
    public ActivityRewardMap()
    {
        Id(x => x.Id);
        Map(x => x.Points);
        Map(x => x.DateEarned);
        References(x => x.User);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}