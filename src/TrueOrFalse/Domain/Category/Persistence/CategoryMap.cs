﻿using FluentNHibernate.Mapping;

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

        References(x => x.Creator).LazyLoad();

        Map(x => x.TopicMarkdown);
        Map(x => x.Content);
        Map(x => x.CustomSegments);

        Map(x => x.CountQuestionsAggregated);

        Map(x => x.CountQuestions);
        Map(x => x.CountSets);

        Map(x => x.Type).CustomType<CategoryType>();
        Map(x => x.TypeJson);

        Map(x => x.CorrectnessProbability);
        Map(x => x.CorrectnessProbabilityAnswerCount);

        Map(x => x.TotalRelevancePersonalEntries);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
        Map(x => x.FormerSetId);
        Map(x => x.SkipMigration);
        Map(x => x.Visibility).CustomType<CategoryVisibility>();
        Map(x => x.IsUserStartTopic);

        Map(x => x.AuthorIds);
    }
}