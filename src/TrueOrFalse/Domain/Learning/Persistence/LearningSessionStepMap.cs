using FluentNHibernate.Mapping;

public class LearningSessionStepMap : ClassMap<LearningSessionStep>
{
    public LearningSessionStepMap()
    {
        Id(x => x.Id);

        References(x => x.Question)
            .Cascade.None();

        References(x => x.AnswerHistory)
            .Cascade.None();

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}