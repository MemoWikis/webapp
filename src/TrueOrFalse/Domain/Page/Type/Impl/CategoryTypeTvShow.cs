using Newtonsoft.Json;

[Serializable]
public class CategoryTypeTvShow : CategoryTypeBase<CategoryTypeTvShow>
{

    [JsonIgnore]
    public override PageType Type { get { return PageType.TvShow; } }
}