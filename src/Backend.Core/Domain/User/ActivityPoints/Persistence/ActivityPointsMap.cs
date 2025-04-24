using FluentNHibernate.Mapping;

public class ActivityPointsMap : ClassMap<ActivityPoints>
{
    public ActivityPointsMap()
    {
        Id(x => x.Id);
        Map(x => x.Amount);
        Map(x => x.DateEarned);
        Map(x => x.UserId).Column("User_id");

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}