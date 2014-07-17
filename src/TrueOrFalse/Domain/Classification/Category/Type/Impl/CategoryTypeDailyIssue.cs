using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrueOrFalse;

[Serializable]
public class CategoryTypeDailyIssue : CategoryTypeBase<CategoryTypeDailyIssue>
{
    public string PublicationDateYear;
    public string Volume;
    public string No;
    public string PublicationDateMonth;
    public string PublicationDateDay;

    /// <summary>Obligatory Daily parent</summary>
    [JsonIgnore]
    public Category Daily
    {
        get
        {
            if (Category == null)
                return null;

            return Category.ParentCategories.FirstOrDefault(c => c.Type == CategoryType.Daily);
        }
    }

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.DailyIssue; } }

    public string BuildTitle(string dailyName)
    {
        var name = "";
        if (!String.IsNullOrEmpty(PublicationDateMonth) && !String.IsNullOrEmpty(PublicationDateDay)) {
            var publicationDate = new DateTime(Convert.ToInt32(PublicationDateYear), Convert.ToInt32(PublicationDateMonth),
            Convert.ToInt32(PublicationDateDay));
            name = dailyName + " vom " + publicationDate.ToString("dd.MM.yyyy");
        }

        return name;
    }
   
}

