using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Seedworks.Lib.Persistence;

[DebuggerDisplay("Id={Id} Name={Name}")]
[Serializable]
public class Category : DomainEntity, ICreator
{
    public virtual string Name { get; set; }

    public virtual string Description { get; set; }

    public virtual string WikipediaURL { get; set; }

    public virtual User Creator { get; set; }
    public virtual IList<Category> ParentCategories { get; set; }

    public virtual IList<Set> FeaturedSets {
        get {
            if (string.IsNullOrEmpty(FeaturedSetsIdsString))
            {
                return new List<Set>();
            }
            var setIds = FeaturedSetsIdsString
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToInt32(x));

            var setRepo = Sl.R<SetRepo>();

            return setIds
                .Select(setId => setRepo.GetById(setId))
                .Where(set => set != null)
                .ToList();
        }
        set
        {
            FeaturedSetsIdsString = value.Count == 0 ? null : string.Join(",", value.Select(x => x.Id.ToString()));
        }
    }

    public virtual string FeaturedSetsIdsString { get; set; }

    public virtual string TopicMarkdown { get; set; }
        
    public virtual int CountQuestions { get; set; }
    public virtual int CountSets { get; set; }
    public virtual int CountCreators { get; set; }

    public virtual CategoryType Type { get; set; }

    public virtual string TypeJson { get; set; }

    public virtual int CorrectnessProbability { get; set; }
    public virtual int CorrectnessProbabilityAnswerCount { get; set; }

    public Category(){
        ParentCategories = new List<Category>();
        Type = CategoryType.Standard;
        if(FeaturedSets == null)
            FeaturedSets = new List<Set>();
    }

    public Category(string name) : this(){
        Name = name;
    }

    public virtual bool IsSpoiler(Question question)
    {
        return IsSpoilerCategory.Yes(Name, question);
    }

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

        throw new Exception("Invalid type.");
    }
}