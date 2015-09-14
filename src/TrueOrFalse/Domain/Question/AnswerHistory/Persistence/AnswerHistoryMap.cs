using FluentNHibernate.Mapping;

public class AnswerHistoryMap : ClassMap<AnswerHistory>
{
    public AnswerHistoryMap()
    {
        Id(x => x.Id);

        Map(x => x.UserId);
        Map(x => x.QuestionId);
        Map(x => x.AnswerText);
        Map(x => x.AnswerredCorrectly);

        References(x => x.Round).Cascade.None();
        References(x => x.Player).Cascade.None();
        References(x => x.LearningSessionStep).Cascade.None().Unique();

        HasManyToMany(x => x.AnswerFeatures).
            Table("answerFeature_to_answerHistory");

        Map(x => x.Milliseconds);
        Map(x => x.DateCreated);
    }           
}
