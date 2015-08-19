using FluentNHibernate.Mapping;

public class LearningSessionMap : ClassMap<LearningSession>
{
    public LearningSessionMap()
    {
        Id(x => x.Id);

        HasMany(x => x.Steps)
            .Cascade.SaveUpdate().OrderBy("Id");
        
        References(x => x.User);
        References(x => x.SetToLearn);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}