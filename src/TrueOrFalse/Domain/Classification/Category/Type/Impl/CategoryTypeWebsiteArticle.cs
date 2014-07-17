using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrueOrFalse;


[Serializable]
public class CategoryTypeWebsiteArticle : CategoryTypeBase<CategoryTypeWebsiteArticle>
{
    public string Title;
    public string Subtitle;
    public string Author;
    public string PublicationDateYear;
    public string PublicationDateMonth;
    public string PublicationDateDay;    
    public string Url;

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.WebsiteArticle; } }
}