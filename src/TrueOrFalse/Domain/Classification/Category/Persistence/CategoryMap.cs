using FluentNHibernate.Mapping;

public class CategoryMap : ClassMap<Category>
{
    public CategoryMap()
    {
        Id(x => x.Id);

        Map(x => x.Name);
        Map(x => x.Description);
        Map(x => x.WikipediaURL);
        Map(x => x.Url);
        Map(x => x.UrlLinkText);

        Map(x => x.CategoriesToExcludeIdsString);
        Map(x => x.CategoriesToIncludeIdsString);

        Map(x => x.DisableLearningFunctions);

        References(x => x.Creator);

        Map(x => x.TopicMarkdown);
        Map(x => x.FeaturedSetsIdsString);

        Map(x => x.CountQuestionsAggregated);

        Map(x => x.CountQuestions);
        Map(x => x.CountSets);

        Map(x => x.Type);
        Map(x => x.TypeJson);

        Map(x => x.CorrectnessProbability);
        Map(x => x.CorrectnessProbabilityAnswerCount);

        Map(x => x.TotalRelevancePersonalEntries);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);

        HasMany(x => x.CategoryRelations).Table("relatedcategoriestorelatedcategories")
            .Cascade.AllDeleteOrphan()
            .Inverse();

    }
}