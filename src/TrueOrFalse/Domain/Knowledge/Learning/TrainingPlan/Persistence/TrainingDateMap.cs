using FluentNHibernate.Mapping;

public class TrainingDateMap : ClassMap<TrainingDate>
{
    public TrainingDateMap()
    {
        Id(x => x.Id);

        Map(x => x.DateTime);

        HasManyToMany(x => x.AllQuestions)
            .Table("trainingdate_questions")
            .Cascade.SaveUpdate();

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}