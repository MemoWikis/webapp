using FluentNHibernate.Mapping;

public class AnswerMap : ClassMap<Answer>
{
    public AnswerMap()
    {
        Id(x => x.Id);
        Map(x => x.UserId);
        References(x => x.Question).Column("QuestionId");
        Map(x => x.QuestionViewGuidString).Column("QuestionViewGuid").CustomSqlType("varchar(36)");
        Map(x => x.InteractionNumber);
        Map(x => x.AnswerText);
        Map(x => x.AnswerredCorrectly).CustomType<AnswerCorrectness>();
        Map(x => x.MillisecondsSinceQuestionView).Column("Milliseconds");
        Map(x => x.Migrated);
        Map(x => x.DateCreated);
    }
}