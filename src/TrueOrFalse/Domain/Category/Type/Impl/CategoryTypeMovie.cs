using Newtonsoft.Json;

[Serializable]
public class CategoryTypeMovie : CategoryTypeBase<CategoryTypeMovie>
{
    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.Movie; } }
}