using System;
using Newtonsoft.Json;

[Serializable]
public class CategoryTypeWebsiteArticle : CategoryTypeBase<CategoryTypeWebsiteArticle>
{
    public string Title;
    public string Subtitle;
    public string Author;
    public string PublicationDateYear;
    public string PublicationDateMonth;
    public string PublicationDateDay;    

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.WebsiteArticle; } }
}