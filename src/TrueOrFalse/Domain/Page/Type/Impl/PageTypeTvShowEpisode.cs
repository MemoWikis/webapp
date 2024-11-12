using Newtonsoft.Json;

[Serializable]
public class PageTypeTvShowEpisode : PageTypeBase<PageTypeTvShowEpisode>
{

    [JsonIgnore]
    public override PageType Type { get { return PageType.TvShowEpisode; } }
}