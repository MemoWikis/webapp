using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Seedworks.Lib.Persistence;

[DebuggerDisplay("Id={Id} Name={Name}")]
[Serializable]
public class UserEntityCacheCategoryItem : DomainEntity
{
    public virtual int Id { get; set; }
    public virtual string Name { get; set; }

    public virtual string Description { get; set; }

    public virtual string WikipediaURL { get; set; }

    public virtual string Url { get; set; }

    public virtual string UrlLinkText { get; set; }

    public virtual bool DisableLearningFunctions { get; set; }

    public virtual User Creator { get; set; }

    public virtual IList<UserEntityCacheCategoryRelations> CategoryRelations { get; set; }
    public virtual int CountQuestions { get; set; }
    public virtual string TopicMarkdown { get; set; }
    public virtual string Content { get; set; }
    public virtual string CustomSegments { get; set; }

    public virtual CategoryType Type { get; set; }

    public virtual string TypeJson { get; set; }

    public virtual int CorrectnessProbability { get; set; }
    public virtual int CorrectnessProbabilityAnswerCount { get; set; }

    public virtual int TotalRelevancePersonalEntries { get; set; }
    public virtual bool IsHistoric { get; set; }

    public virtual int FormerSetId { get; set; }
    public virtual bool SkipMigration { get; set; }

    public virtual bool IsRootCategory => Id == RootCategory.RootCategoryId;


    public virtual IList<Category> ParentCategories()
    {
        return CategoryRelations.Any()
            ? CategoryRelations
                .Where(r => r.CategoryRelationType == CategoryRelationType.IsChildCategoryOf)
                .Select(x => EntityCache.GetCategory(x.RelatedCategory.Id))
                .ToList()
            : new List<Category>();
    }

    public virtual UserEntityCachedCategoryData CachedData { get; set; } = new UserEntityCachedCategoryData();

    public virtual string CategoriesToExcludeIdsString { get; set; }

    private IEnumerable<int> _categoriesToExcludeIds;
    public virtual IEnumerable<int> CategoriesToExcludeIds() =>
        _categoriesToExcludeIds ?? (_categoriesToExcludeIds = CategoriesToExcludeIdsString
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Convert.ToInt32(x)));


    private IEnumerable<int> _categoriesToIncludeIds;
    public virtual string CategoriesToIncludeIdsString { get; set; }
    public virtual IEnumerable<int> CategoriesToIncludeIds() =>
        _categoriesToIncludeIds ?? (_categoriesToIncludeIds = CategoriesToIncludeIdsString
            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Convert.ToInt32(x)));

    public virtual IList<Category> CategoriesToInclude()
    {
        return !string.IsNullOrEmpty(CategoriesToIncludeIdsString)
            ? Sl.R<CategoryRepository>().GetByIdsFromString(CategoriesToIncludeIdsString)
            : new List<Category>();
    }

    public virtual IList<Category> CategoriesToExclude()
    {
        return !string.IsNullOrEmpty(CategoriesToExcludeIdsString)
            ? Sl.R<CategoryRepository>().GetByIdsFromString(CategoriesToExcludeIdsString)
            : new List<Category>();
    }

    public virtual IList<UserEntityCacheCategoryItem> AggregatedCategories(bool includingSelf = true)
    {
        var list = new List<UserEntityCacheCategoryItem>();

        if (UserCache.GetItem(Sl.CurrentUserId).IsFiltered)
        {
            list = EntityCache
                .GetCategory(Id, getDataFromEntityCache: true).CategoryRelations
                .Where(r => r.RelatedCategory
                .IsInWishknowledge() && r.CategoryRelationType == CategoryRelationType.IncludesContentOf)
                .Select(r => ToCacheCategoryItem(EntityCache.GetCategory(r.RelatedCategory.Id))).ToList();
        }
        else
            list = CategoryRelations.Where(r => r.CategoryRelationType == CategoryRelationType.IncludesContentOf)
                .Select(r =>ToCacheCategoryItem(EntityCache.GetCategory(r.RelatedCategory.Id))).ToList();

        if (includingSelf)
            list.Add(this);

        return list;
    }
    public virtual int CountQuestionsAggregated { get; set; }

    public virtual void UpdateCountQuestionsAggregated()
    {
        CountQuestionsAggregated = GetCountQuestionsAggregated();
    }

    public virtual int GetCountQuestionsAggregated(bool inCategoryOnly = false, int categoryId = 0)
    {
        if (inCategoryOnly)
            return GetAggregatedQuestionsFromMemoryCache(true, false, categoryId).Count;

        return GetAggregatedQuestionsFromMemoryCache().Count;
    }

    public virtual IList<Question> GetAggregatedQuestionsFromMemoryCache(bool onlyVisible = true, bool fullList = true, int categoryId = 0)
    {
        IEnumerable<Question> questions;

        if (fullList)
        {
            questions = AggregatedCategories()
                .SelectMany(c => EntityCache.GetQuestionsForCategory(c.Id))
                .Distinct();
        }
        else
        {
            questions = EntityCache.GetQuestionsForCategory(categoryId)
                .Distinct();
        }


        if (onlyVisible)
        {
            questions = questions.Where(q => q.IsVisibleToCurrentUser());
        }

        return questions.ToList();
    }

    public virtual IList<int> GetAggregatedQuestionIdsFromMemoryCache()
    {
        return AggregatedCategories()
            .SelectMany(c => EntityCache.GetQuestionsIdsForCategory(c.Id))
            .Distinct()
            .ToList();
    }

    public virtual bool IsInWishknowledge() => UserCache.IsInWishknowledge(Sl.CurrentUserId, Id);

    public UserEntityCacheCategoryItem()
    {
        CategoryRelations = new List<UserEntityCacheCategoryRelations>();
    }

    public UserEntityCacheCategoryItem(string name) : this()
    {
        Name = name;
    }

    public virtual bool IsSpoiler(Question question) =>
        IsSpoilerCategory.Yes(Name, question);

    public UserEntityCacheCategoryItem ToCacheCategoryItem(Category category)
    {
        var c = category.CategoryRelations;
        return new UserEntityCacheCategoryItem
        {
            Id = category.Id,
            CachedData = new UserEntityCachedCategoryData().ToCachedCategoryData(category.CachedData) ,
            CategoryRelations = new UserEntityCacheCategoryRelations().ToListCategoryRelations(category.CategoryRelations),
            CategoriesToExcludeIdsString = category.CategoriesToExcludeIdsString,
            CategoriesToIncludeIdsString = CategoriesToIncludeIdsString,
            Content = category.Content,
            CorrectnessProbability = category.CorrectnessProbability,
            CorrectnessProbabilityAnswerCount = category.CorrectnessProbabilityAnswerCount,
            CountQuestions = category.CountQuestions,
            CountQuestionsAggregated = category.CountQuestionsAggregated,
            Creator = category.Creator,
            CustomSegments = category.CustomSegments,
            Description = category.Description,
            DisableLearningFunctions = category.DisableLearningFunctions,
            FormerSetId = category.FormerSetId,
            IsHistoric = category.IsHistoric,
            Name = category.Name,
            SkipMigration = category.SkipMigration,
            TopicMarkdown = category.TopicMarkdown,
            TotalRelevancePersonalEntries = 50,
            Type = category.Type,
            TypeJson = category.TypeJson,
            Url = category.Url,
            UrlLinkText = category.UrlLinkText,
            WikipediaURL = category.WikipediaURL

        };
    }
}


