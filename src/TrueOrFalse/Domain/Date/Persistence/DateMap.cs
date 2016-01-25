using FluentNHibernate.Mapping;

public class DateMap : ClassMap<Date>
{
    public DateMap()
    {
        Id(x => x.Id);

        Map(x => x.Visibility);
        Map(x => x.Details);

        References(x => x.TrainingPlan).Cascade.All();
        References(x => x.User);

        HasManyToMany(x => x.Sets)
            .Table("date_to_sets")
            .Cascade.None();

        Map(x => x.DateTime);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}