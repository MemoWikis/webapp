using Seedworks.Lib.Persistence;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;

[DebuggerDisplay("Id={Id} Name={Name}")]
[Serializable]
public class Category : DomainEntity, ICreator, ICloneable
{
    public virtual string Name { get; set; }

    public virtual string Description { get; set; }

    public virtual string WikipediaURL { get; set; }

    public virtual string Url { get; set; }

    public virtual string UrlLinkText { get; set; }

    public virtual bool DisableLearningFunctions { get; set; }

    public virtual User Creator { get; set; }

    public virtual bool IsUserStartTopic { get; set; }

    public virtual string AuthorIds { get; set; } = "";

    public virtual int[] AuthorIdsInts => AuthorIds?
        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(x => Convert.ToInt32(x)).Distinct()
        .ToArray();

    public virtual string CategoriesToExcludeIdsString { get; set; }

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

    public virtual int CountQuestions { get; set; }
    public virtual int CountSets { get; set; }
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

    public Category()
    {
        Type = CategoryType.Standard;
    }

    public Category(string name, int userId) : this()
    {
        Name = name;
        AuthorIds = userId + ",";
    }

    public virtual bool IsSpoiler(QuestionCacheItem question) =>
        IsSpoilerCategory.Yes(Name, question);

    public virtual object GetTypeModel()
    {
        if (Type == CategoryType.Standard)
            return CategoryTypeStandard.FromJson(this);

        if (Type == CategoryType.Book)
            return CategoryTypeBook.FromJson(this);

        if (Type == CategoryType.Daily)
            return CategoryTypeDaily.FromJson(this);

        if (Type == CategoryType.DailyArticle)
            return CategoryTypeDailyArticle.FromJson(this);

        if (Type == CategoryType.DailyIssue)
            return CategoryTypeDailyIssue.FromJson(this);

        if (Type == CategoryType.Magazine)
            return CategoryTypeMagazine.FromJson(this);

        if (Type == CategoryType.MagazineArticle)
            return CategoryTypeMagazineArticle.FromJson(this);

        if (Type == CategoryType.MagazineIssue)
            return CategoryTypeMagazineIssue.FromJson(this);

        if (Type == CategoryType.VolumeChapter)
            return CategoryTypeVolumeChapter.FromJson(this);

        if (Type == CategoryType.Website)
            return CategoryTypeWebsite.FromJson(this);

        if (Type == CategoryType.WebsiteArticle)
            return CategoryTypeWebsiteArticle.FromJson(this);

        if (Type == CategoryType.WebsiteVideo)
            return CategoryTypeWebsiteVideo.FromJson(this);

        if (Type == CategoryType.SchoolSubject)
            return CategoryTypeSchoolSubject.FromJson(this);

        if (Type == CategoryType.FieldOfStudy)
            return CategoryTypeFieldOfStudy.FromJson(this);

        if (Type == CategoryType.FieldOfTraining)
            return CategoryTypeFieldOfTraining.FromJson(this);

        if (Type == CategoryType.EducationProvider)
            return CategoryTypeEducationProvider.FromJson(this);

        if (Type == CategoryType.Course)
            return CategoryTypeCourse.FromJson(this);

        throw new Exception("Invalid type.");
    }

    public virtual int FormerSetId { get; set; }
    public virtual bool SkipMigration { get; set; }

    public virtual object Clone()
    {
        return this.MemberwiseClone();
    }
}