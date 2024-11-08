using Newtonsoft.Json;

[Serializable]
public class CategoryTypeWebsiteArticle : CategoryTypeBase<CategoryTypeWebsiteArticle>
{
    public string Title;
    public string Author;
   

    [JsonIgnore]
    public override PageType Type { get { return PageType.WebsiteArticle; } }
}