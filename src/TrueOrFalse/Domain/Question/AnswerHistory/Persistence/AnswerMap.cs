using System;
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
        Map(x => x.AnswerredCorrectly);

        References(x => x.LearningSession).Cascade.None();
        Map(x => x.LearningSessionStepGuidString).Column("LearningSessionStepGuid").CustomSqlType("varchar(36)").Unique();
        
        HasManyToMany(x => x.Features).
            Table("answerFeature_to_answer");

        Map(x => x.MillisecondsSinceQuestionView).Column("Milliseconds");

        Map(x => x.Migrated);

        Map(x => x.DateCreated);
    }           
}
