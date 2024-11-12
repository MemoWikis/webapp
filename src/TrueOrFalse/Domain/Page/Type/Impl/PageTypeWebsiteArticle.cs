using Newtonsoft.Json;

[Serializable]
public class PageTypeWebsiteArticle : PageTypeBase<PageTypeWebsiteArticle>
{
    public string Title;
    public string Author;
   

    [JsonIgnore]
    public override PageType Type { get { return PageType.WebsiteArticle; } }
}