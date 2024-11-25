using FluentNHibernate.Mapping;
using TrueOrFalse;

public class QuestionMap : ClassMap<Question>
{
    public QuestionMap()
    {
        Id(x => x.Id);

        Map(x => x.Text)
            .CustomSqlType("VARCHAR(3000)").Length(3000);

        Map(x => x.TextHtml).CustomSqlType("MEDIUMTEXT");
        Map(x => x.TextExtended).CustomSqlType("TEXT");
        Map(x => x.TextExtendedHtml).CustomSqlType("TEXT");
        Map(x => x.Description).CustomSqlType("TEXT");
        Map(x => x.DescriptionHtml).CustomSqlType("TEXT");
        Map(x => x.LicenseId).Column("License");
        Map(x => x.Visibility).CustomType<QuestionVisibility>();
        References(x => x.Creator);

        Map(x => x.TotalTrueAnswers);
        Map(x => x.TotalFalseAnswers);

        Map(x => x.TotalQualityAvg);
        Map(x => x.TotalQualityEntries);

        Map(x => x.TotalRelevanceForAllAvg);
        Map(x => x.TotalRelevanceForAllEntries);

        Map(x => x.TotalRelevancePersonalAvg);
        Map(x => x.TotalRelevancePersonalEntries);

        Map(x => x.CorrectnessProbability);
        Map(x => x.CorrectnessProbabilityAnswerCount);

        Map(x => x.Solution).CustomSqlType("MEDIUMTEXT");
        Map(x => x.SolutionType).CustomType<SolutionType>();
        Map(x => x.SolutionMetadataJson).CustomSqlType("VARCHAR(7000)").Length(7000);

        HasMany(x => x.References).Cascade.AllDeleteOrphan();
        HasManyToMany(x => x.Pages)
            .Table("categories_to_questions")
            .ParentKeyColumn("Question_id")
            .ChildKeyColumn("Page_id")
            .Cascade.SaveUpdate();

        Map(x => x.IsWorkInProgress);
        Map(x => x.DateCreated);
        Map(x => x.DateModified);
        Map(x => x.SkipMigration);
    }
}