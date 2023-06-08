using Newtonsoft.Json;

[Serializable]
public class CategoryTypeWebsite : CategoryTypeBase<CategoryTypeWebsite>
{
    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.Website; } }
}