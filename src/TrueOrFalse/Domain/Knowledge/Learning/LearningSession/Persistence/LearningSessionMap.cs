using FluentNHibernate.Mapping;

public class LearningSessionMap : ClassMap<LearningSession>
{
    public LearningSessionMap()
    {
        Id(x => x.Id);

        HasMany(x => x.Steps)
            .Cascade.SaveUpdate().OrderBy("Idx");

        References(x => x.User);

        References(x => x.SetToLearn);
        References(x => x.DateToLearn);

        Map(x => x.IsCompleted);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}