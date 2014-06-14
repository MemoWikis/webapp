using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

[Serializable]
public class CategoryDailyIssue : CategoryBase<CategoryDailyIssue>
{
    public string Year;
    public string Volume;
    public string No;
    public string PublicationDateMonth;
    public string PublicationDateDay;

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.DailyIssue; } }

    public string BuildTitle()
    {
        var name = "";
        if (!String.IsNullOrEmpty(PublicationDateMonth) && !String.IsNullOrEmpty(PublicationDateDay)) { 
            var publicationDate = new DateTime(Convert.ToInt32(Year), Convert.ToInt32(PublicationDateMonth),
            Convert.ToInt32(PublicationDateDay));
            name = publicationDate.ToString("dd.MM.yyyy");
        }

        return name;
    }
   
}

