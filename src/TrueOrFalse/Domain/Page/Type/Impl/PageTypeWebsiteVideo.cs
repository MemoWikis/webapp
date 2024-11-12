using Newtonsoft.Json;

[Serializable]
public class PageTypeWebsiteVideo : PageTypeBase<PageTypeWebsiteVideo>
{

    [JsonIgnore]
    public override PageType Type { get { return PageType.WebsiteVideo; } }
}