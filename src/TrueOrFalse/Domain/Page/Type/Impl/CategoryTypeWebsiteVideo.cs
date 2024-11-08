using Newtonsoft.Json;

[Serializable]
public class CategoryTypeWebsiteVideo : CategoryTypeBase<CategoryTypeWebsiteVideo>
{

    [JsonIgnore]
    public override PageType Type { get { return PageType.WebsiteVideo; } }
}