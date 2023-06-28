using Newtonsoft.Json;

[Serializable]
public class CategoryTypeWebsiteVideo : CategoryTypeBase<CategoryTypeWebsiteVideo>
{

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.WebsiteVideo; } }
}