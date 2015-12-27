using FluentNHibernate.Mapping;

public class TrainingQuestionMap : ClassMap<TrainingQuestion>
{
    public TrainingQuestionMap()
    {
        Id(x => x.Id);

        References(x => x.Question);
        Map(x => x.ProbBefore);
        Map(x => x.ProbAfter);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}