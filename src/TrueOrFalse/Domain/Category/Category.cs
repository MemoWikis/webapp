using Seedworks.Lib.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TrueOrFalse.Tools;

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

    public virtual IList<CategoryRelation> CategoryRelations { get; set; }

    public virtual IList<Category> ParentCategories()
    {
        return CategoryRelations.Any()
            ? CategoryRelations
                .Where(r => r.CategoryRelationType == CategoryRelationType.IsChildCategoryOf)
                .Select(x => EntityCache.GetCategory(x.RelatedCategory.Id))
                .ToList()
            : new List<Category>();
    }

    public virtual CategoryCachedData CachedData { get; set; } = new CategoryCachedData();

    public virtual string CategoriesToExcludeIdsString { get; set; }

    private IEnumerable<int> _categoriesToExcludeIds;
    public virtual IEnumerable<int> CategoriesToExcludeIds() =>
        _categoriesToExcludeIds ?? (_categoriesToExcludeIds = CategoriesToExcludeIdsString
            .Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
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

    public virtual IList<Category> AggregatedCategories(bool includingSelf = true)
    {
        var list = CategoryRelations.Where(r => r.CategoryRelationType == CategoryRelationType.IncludesContentOf)
            .Select(r => EntityCache.GetCategory(r.RelatedCategory.Id) ).ToList();
        
        if(includingSelf)
            list.Add(this);

        return list;
    }

    public virtual IList<Category> NonAggregatedCategories()
    {
        return Sl.R<CategoryRepository>()
            .GetDescendants(Id)
            .Except(AggregatedCategories(includingSelf: false))
            .Except(CategoriesToExclude())
            .Distinct()
            .ToList();
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

    public virtual int CountQuestions { get; set; }
    public virtual int CountSets { get; set; }

    public virtual string FeaturedSetsIdsString { get; set; }

    public virtual string TopicMarkdown { get; set; }
    public virtual string Content { get; set; }
    public virtual string CustomSegments { get; set; }

    public virtual CategoryType Type { get; set; }

    public virtual string TypeJson { get; set; }

    public virtual int CorrectnessProbability { get; set; }
    public virtual int CorrectnessProbabilityAnswerCount { get; set; }

    public virtual int TotalRelevancePersonalEntries { get; set; }
    public virtual bool IsHistoric { get; set; }

    public virtual bool IsInWishknowledge()
    {
        return Sl.CategoryValuationRepo.IsInWishKnowledge(Id, Sl.CurrentUserId);
    }

    public Category(){
        CategoryRelations = new List<CategoryRelation>();
        Type = CategoryType.Standard;
    }

    public Category(string name) : this(){
        Name = name;
    }

    public virtual bool IsSpoiler(Question question) => 
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

    public virtual string ToLomXml() => LomXml.From(this);

    public virtual int FormerSetId { get; set; }
    public virtual bool SkipMigration { get; set; }

    public virtual object Clone()
    {
        return this.MemberwiseClone(); 
    }

    public virtual bool IsRootCategory => Id == RootCategory.RootCategoryId;
}