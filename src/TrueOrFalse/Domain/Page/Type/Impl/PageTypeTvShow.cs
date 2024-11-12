using Newtonsoft.Json;

[Serializable]
public class PageTypeTvShow : PageTypeBase<PageTypeTvShow>
{

    [JsonIgnore]
    public override PageType Type { get { return PageType.TvShow; } }
}