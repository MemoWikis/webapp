using Newtonsoft.Json;

[Serializable]
public class PageTypeWebsite : PageTypeBase<PageTypeWebsite>
{
    [JsonIgnore]
    public override PageType Type { get { return PageType.Website; } }
}