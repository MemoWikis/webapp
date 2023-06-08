using Newtonsoft.Json;

[Serializable]
public class CategoryTypeTvShow : CategoryTypeBase<CategoryTypeTvShow>
{

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.TvShow; } }
}