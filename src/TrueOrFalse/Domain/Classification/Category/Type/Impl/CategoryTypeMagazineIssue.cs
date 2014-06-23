using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

[Serializable]
public class CategoryTypeMagazineIssue : CategoryTypeBase<CategoryTypeMagazineIssue>
{
    public string Year;
    public string Volume;
    public string No;
    public string IssuePeriod;
    public string PublicationDateMonth;
    public string PublicationDateDay;
    public string Title;

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.MagazineIssue; } }

    public string BuildTitle()
    {
        var name = "";
        if (!String.IsNullOrEmpty(Year)) {
            name = Year;
            if (!String.IsNullOrEmpty(No))
                name += "/" + No;
            if (!String.IsNullOrEmpty(IssuePeriod))
                name += " (" + IssuePeriod + ")";
        }

        return name;
    }
}

