using System;
using System.Collections.Generic;
using System.Diagnostics;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    [DebuggerDisplay("Id={Id} Name={Name}")]
    [Serializable]
    public class Category : DomainEntity
    {
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string WikipediaURL { get; set; }

        public virtual User Creator { get; set; }
        public virtual IList<Category> ParentCategories { get; set; }
        
        public virtual int CountQuestions { get; set; }
        public virtual int CountSets { get; set; }
        public virtual int CountCreators { get; set; }

        public virtual CategoryType Type { get; set; }

        public virtual string TypeJson { get; set; }

        public Category(){
            ParentCategories = new List<Category>();
            Type = CategoryType.Standard;
        }

        public Category(string name) : this(){
            Name = name;
        }

        public virtual object GetTypeModel()
        {
            if (Type == CategoryType.Standard)
                return new object();

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
}