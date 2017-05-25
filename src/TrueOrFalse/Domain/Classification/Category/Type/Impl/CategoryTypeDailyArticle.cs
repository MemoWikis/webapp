using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrueOrFalse;

[Serializable]
public class CategoryTypeDailyArticle : CategoryTypeBase<CategoryTypeDailyArticle>
{
    public string Title;
    public string Subtitle;
    public string Author;
    public string Url;
    public string PagesArticleFrom;
    public string PagesArticleTo;

    /// <summary>Obligatory Daily parent</summary>
    [JsonIgnore]
    public Category Daily
    {
        get
        {
            if (Category == null)
                return null;

            return Category.ParentCategories().FirstOrDefault(c => c.Type == CategoryType.Daily);
        }
    }

    /// <summary>Obligatory DailyIssue parent</summary>
    [JsonIgnore]
    public Category DailyIssue
    {
        get
        {
            if (Category == null)
                return null;

            return Category.ParentCategories().FirstOrDefault(c => c.Type == CategoryType.DailyIssue);
        }
    }

     [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.DailyArticle; } }
}

