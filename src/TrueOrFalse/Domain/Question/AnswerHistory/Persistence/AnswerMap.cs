using FluentNHibernate.Mapping;

public class AnswerMap : ClassMap<Answer>
{
    public AnswerMap()
    {
        Id(x => x.Id);

        Map(x => x.UserId);

        References(x => x.Question).Column("QuestionId");

        Map(x => x.AnswerText);
        Map(x => x.AnswerredCorrectly);

        References(x => x.Round).Cascade.None();
        References(x => x.Player).Cascade.None();
        References(x => x.LearningSession).Cascade.None();
        Map(x => x.LearningSessionStepGuid).Unique();

        HasManyToMany(x => x.Features).
            Table("answerFeature_to_answer");

        Map(x => x.Milliseconds);
        Map(x => x.DateCreated);
    }           
}
