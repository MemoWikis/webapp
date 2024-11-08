using Newtonsoft.Json;

[Serializable]
public class CategoryTypeWebsite : CategoryTypeBase<CategoryTypeWebsite>
{
    [JsonIgnore]
    public override PageType Type { get { return PageType.Website; } }
}