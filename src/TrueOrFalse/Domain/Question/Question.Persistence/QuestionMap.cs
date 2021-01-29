using FluentNHibernate.Mapping;

public class QuestionMap : ClassMap<Question>
{
    public QuestionMap()
    {
        Id(x => x.Id);
        Map(x => x.Text).Length(Constants.VarCharMaxLength);
        Map(x => x.TextExtended).Length(Constants.VarCharMaxLength);
        Map(x => x.Description).Length(Constants.VarCharMaxLength);
        Map(x => x.LicenseId).Column("License");
        Map(x => x.Visibility);
        References(x => x.Creator);

        Map(x => x.TotalTrueAnswers);
        Map(x => x.TotalFalseAnswers);

        Map(x => x.TotalQualityAvg);
        Map(x => x.TotalQualityEntries);
            
        Map(x => x.TotalRelevanceForAllAvg);
        Map(x => x.TotalRelevanceForAllEntries);
            
        Map(x => x.TotalRelevancePersonalAvg);
        Map(x => x.TotalRelevancePersonalEntries);
        Map(x => x.TotalViews);

        Map(x => x.CorrectnessProbability);
        Map(x => x.CorrectnessProbabilityAnswerCount);

        Map(x => x.Solution);
        Map(x => x.SolutionType);
        Map(x => x.SolutionMetadataJson);

        HasMany(x => x.References).Cascade.AllDeleteOrphan();
        HasManyToMany(x => x.Categories)
            .Table("categories_to_questions")
            .Cascade.SaveUpdate();

        Map(x => x.IsWorkInProgress);

        //HasManyToMany(x => x.Features)
        //    .Table("questionFeature_to_question")
        //    .Inverse();

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}