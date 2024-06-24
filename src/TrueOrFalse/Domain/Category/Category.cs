using Seedworks.Lib.Persistence;
using System.Diagnostics;

[DebuggerDisplay("Id={Id} Name={Name}")]
[Serializable]
public class Category : DomainEntity, ICreator
{
    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
    public virtual string WikipediaURL { get; set; }
    public virtual string Url { get; set; }
    public virtual string UrlLinkText { get; set; }
    public virtual bool DisableLearningFunctions { get; set; }
    public virtual User Creator { get; set; }
    public virtual bool IsUserStartTopic { get; set; }
    public virtual bool TextIsHidden { get; set; }
    public virtual string AuthorIds { get; set; } = "";
    public virtual string CategoriesToExcludeIdsString { get; set; }

    public virtual int[] AuthorIdsInts => AuthorIds?
        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(x => Convert.ToInt32(x)).Distinct()
        .ToArray();

    private IEnumerable<int> _categoriesToExcludeIds;

    public virtual IEnumerable<int> CategoriesToExcludeIds() =>
        _categoriesToExcludeIds ??= CategoriesToExcludeIdsString
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Convert.ToInt32(x));

    private IEnumerable<int> _categoriesToIncludeIds;
    public virtual string CategoriesToIncludeIdsString { get; set; }

    public virtual IEnumerable<int> CategoriesToIncludeIds() =>
        _categoriesToIncludeIds ??= CategoriesToIncludeIdsString
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Convert.ToInt32(x));

    public virtual int CountQuestionsAggregated { get; set; }

    public virtual void UpdateCountQuestionsAggregated(int userId)
    {
        var categoryCacheItem = EntityCache.GetCategory(Id);
        if (categoryCacheItem != null)
            CountQuestionsAggregated = categoryCacheItem.GetCountQuestionsAggregated(userId);
    }

    public virtual string TopicMarkdown { get; set; }
    public virtual string Content { get; set; }
    public virtual string CustomSegments { get; set; }
    public virtual CategoryType Type { get; set; }
    public virtual string TypeJson { get; set; }
    public virtual int CorrectnessProbability { get; set; }
    public virtual int CorrectnessProbabilityAnswerCount { get; set; }
    public virtual int TotalRelevancePersonalEntries { get; set; }
    public virtual bool IsHistoric { get; set; }
    public virtual CategoryVisibility Visibility { get; set; }
    public virtual bool SkipMigration { get; set; }

    public Category()
    {
        Type = CategoryType.Standard;
    }

    public Category(string name, int userId) : this()
    {
        Name = name;
        AuthorIds = userId + ",";
    }
}