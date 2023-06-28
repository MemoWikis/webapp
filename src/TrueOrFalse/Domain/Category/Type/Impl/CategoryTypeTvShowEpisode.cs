using Newtonsoft.Json;

[Serializable]
public class CategoryTypeTvShowEpisode : CategoryTypeBase<CategoryTypeTvShowEpisode>
{

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.TvShowEpisode; } }
}