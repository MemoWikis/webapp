using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CategoryMagazineIssue : CategoryBase<CategoryMagazineIssue>
{
    public int Year;
    public int Volume;
    public int No;
    public string IssuePeriod;
    public int PublicationDateMonth;
    public int PublicationDateDay;
    public string Title;
}

