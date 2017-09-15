using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrueOrFalse;

[Serializable]
public class CategoryTypeMagazineArticle : CategoryTypeBase<CategoryTypeMagazineArticle>
{
    public string Title;
    public string Subtitle;
    public string Author;
    public string PagesArticleFrom;
    public string PagesArticleTo;

    /// <summary>Obligatory Daily parent</summary>
    [JsonIgnore]
    public Category Magazine
    {
        get
        {
            if (Category == null)
                return null;

            return Category.ParentCategories().FirstOrDefault(c => c.Type == CategoryType.Magazine);
        }
    }

    /// <summary>Obligatory DailyIssue parent</summary>
    [JsonIgnore]
    public Category MagazineIssue
    {
        get
        {
            if (Category == null)
                return null;

            return Category.ParentCategories().FirstOrDefault(c => c.Type == CategoryType.MagazineIssue);
        }
    }

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.MagazineArticle; } }
}

