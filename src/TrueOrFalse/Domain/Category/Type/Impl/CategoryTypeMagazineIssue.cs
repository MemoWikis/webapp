using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrueOrFalse;

[Serializable]
public class CategoryTypeMagazineIssue : CategoryTypeBase<CategoryTypeMagazineIssue>
{
    public string PublicationDateYear;
    public string Volume;
    public string No;
    public string IssuePeriod;
    public string PublicationDateMonth;
    public string PublicationDateDay;
    public string Title;

    /// <summary>Obligatory Magazine parent</summary>
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

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.MagazineIssue; } }

    public string BuildTitle(string magazineId)
    {
        var name = "";
        var magazineName = ServiceLocator.Resolve<CategoryRepository>().GetById(Int32.Parse(magazineId)).Name;
        if (!String.IsNullOrEmpty(No) && !String.IsNullOrEmpty(PublicationDateYear))
        {
            name = magazineName + " " + No + "/" + PublicationDateYear;
            if (!String.IsNullOrEmpty(IssuePeriod))
                name += " (" + IssuePeriod + ")";
        }

        return name;
    }
}

