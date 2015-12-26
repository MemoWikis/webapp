using FluentNHibernate.Mapping;

public class TrainingDateMap : ClassMap<TrainingDate>
{
    public TrainingDateMap()
    {
        Id(x => x.Id);

        Map(x => x.DateTime);

        HasManyToMany(x => x.Questions)
            .Table("trainingdate_questions")
            .Cascade.None();

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}