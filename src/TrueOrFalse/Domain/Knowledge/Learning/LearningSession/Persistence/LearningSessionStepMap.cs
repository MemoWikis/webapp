using FluentNHibernate.Mapping;

public class LearningSessionStepMap : ClassMap<LearningSessionStep>
{
    public LearningSessionStepMap()
    {
        Id(x => x.Id);

        References(x => x.Question)
            .Cascade.None();

        Map(x => x.Idx);

        Map(x => x.AnswerState);

        HasOne(x => x.AnswerHistory).PropertyRef(x => x.LearningSessionStep);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}