using FluentNHibernate.Mapping;

public class DateMap : ClassMap<Date>
{
    public DateMap()
    {
        Id(x => x.Id);

        Map(x => x.Visibility);
        Map(x => x.Details);

        References(x => x.TrainingPlan).Cascade.All();
        Map(x => x.TrainingPlanJson);

        References(x => x.User);

        HasMany(x => x.LearningSessions).Table("learningsession").KeyColumn("DateToLearn_id"); //should define cascade-relation

        HasManyToMany(x => x.Sets)
            .Table("date_to_sets")
            .Cascade.None();

        Map(x => x.DateTime);

        References(x => x.CopiedFrom).Column("CopiedFrom").Cascade.None(); //if parent is deleted, child remains and its column "CopiedFrom" is set to NULL
        HasMany(x => x.CopiedInstances).Cascade.None().KeyColumn("CopiedFrom");


        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}