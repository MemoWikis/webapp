using Newtonsoft.Json;

[Serializable]
public class CategoryTypeWebsiteArticle : CategoryTypeBase<CategoryTypeWebsiteArticle>
{
    public string Title;
    public string Author;
   

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.WebsiteArticle; } }
}