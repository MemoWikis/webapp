using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CategoryMagazineIssue : CategoryBase<CategoryMagazineIssue>
{
    public string Year;
    public string Volume;
    public string No;
    public string IssuePeriod;
    public string PublicationDateMonth;
    public string PublicationDateDay;
    public string Title;

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

