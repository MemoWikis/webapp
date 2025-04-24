using Newtonsoft.Json;

[Serializable]
public class PageTypeMagazineArticle : PageTypeBase<PageTypeMagazineArticle>
{
    public string Title;
    public string Author;

    [JsonIgnore]
    public override PageType Type
    {
        get { return PageType.MagazineArticle; }
    }
}