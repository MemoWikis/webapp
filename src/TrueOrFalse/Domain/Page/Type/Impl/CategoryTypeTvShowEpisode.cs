using Newtonsoft.Json;

[Serializable]
public class CategoryTypeTvShowEpisode : CategoryTypeBase<CategoryTypeTvShowEpisode>
{

    [JsonIgnore]
    public override PageType Type { get { return PageType.TvShowEpisode; } }
}