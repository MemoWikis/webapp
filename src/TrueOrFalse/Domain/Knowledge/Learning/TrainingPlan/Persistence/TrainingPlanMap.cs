using FluentNHibernate.Mapping;

public class TrainingPlanMap : ClassMap<TrainingPlan>
{
    public TrainingPlanMap()
    {
        Id(x => x.Id);

        References(x => x.Date);
        HasMany(x => x.Dates).Cascade.SaveUpdate();

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}