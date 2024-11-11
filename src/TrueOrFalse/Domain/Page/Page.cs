﻿using Seedworks.Lib.Persistence;
using System.Diagnostics;

[DebuggerDisplay("Id={Id} Name={Name}")]
[Serializable]
public class Page : DomainEntity, ICreator
{
    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
    public virtual string WikipediaURL { get; set; }
    public virtual string Url { get; set; }
    public virtual string UrlLinkText { get; set; }
    public virtual bool DisableLearningFunctions { get; set; }
    public virtual User Creator { get; set; }
    public virtual bool IsUserStartPage { get; set; }
    public virtual bool TextIsHidden { get; set; }
    public virtual string AuthorIds { get; set; } = "";
    public virtual string CategoriesToExcludeIdsString { get; set; }

    public virtual int[] AuthorIdsInts => AuthorIds?
        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(x => Convert.ToInt32(x)).Distinct()
        .ToArray();

    public virtual string CategoriesToIncludeIdsString { get; set; }

    public virtual int CountQuestionsAggregated { get; set; }

    public virtual void UpdateCountQuestionsAggregated(int userId)
    {
        var categoryCacheItem = EntityCache.GetPage(Id);
        if (categoryCacheItem != null)
            CountQuestionsAggregated = categoryCacheItem.GetCountQuestionsAggregated(userId);
    }

    public virtual string Markdown { get; set; }
    public virtual string Content { get; set; }
    public virtual string CustomSegments { get; set; }
    public virtual PageType Type { get; set; }
    public virtual string TypeJson { get; set; }
    public virtual int CorrectnessProbability { get; set; }
    public virtual int CorrectnessProbabilityAnswerCount { get; set; }
    public virtual int TotalRelevancePersonalEntries { get; set; }
    public virtual bool IsHistoric { get; set; }
    public virtual PageVisibility Visibility { get; set; }
    public virtual bool SkipMigration { get; set; }

    public Page()
    {
        Type = PageType.Standard;
    }

    public Page(string name, int userId) : this()
    {
        Name = name;
        AuthorIds = userId + ",";
    }
}