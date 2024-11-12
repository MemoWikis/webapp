using Newtonsoft.Json;

[Serializable]
public class PageTypeDailyArticle : PageTypeBase<PageTypeDailyArticle>
{
    public string Title;
    public string Author;

    [JsonIgnore]
    public override PageType Type
    {
        get { return PageType.DailyArticle; }
    }
}