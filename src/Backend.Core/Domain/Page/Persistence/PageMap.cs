using FluentNHibernate.Mapping;

public class PageMap : ClassMap<Page>
{
    public PageMap()
    {
        Table("page");
        Id(x => x.Id);

        Map(x => x.Name);
        Map(x => x.Description);
        Map(x => x.WikipediaURL);
        Map(x => x.Url);
        Map(x => x.UrlLinkText);
        Map(x => x.DisableLearningFunctions);
        References(x => x.Creator).LazyLoad();
        Map(x => x.Markdown).Column("TopicMarkdown");
        Map(x => x.Content).CustomSqlType("MEDIUMTEXT");
        Map(x => x.CustomSegments);
        Map(x => x.CountQuestionsAggregated);
        Map(x => x.CorrectnessProbability);
        Map(x => x.CorrectnessProbabilityAnswerCount);
        Map(x => x.TotalRelevancePersonalEntries);
        Map(x => x.DateCreated);
        Map(x => x.DateModified);
        Map(x => x.SkipMigration);
        Map(x => x.Visibility).CustomType<PageVisibility>();
        Map(x => x.IsWiki);
        Map(x => x.TextIsHidden).CustomType<bool>();
        Map(x => x.AuthorIds);
        Map(x => x.Language);
    }
}