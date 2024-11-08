using Newtonsoft.Json;

[Serializable]
public class CategoryTypeStandard : CategoryTypeBase<CategoryTypeStandard>
{
    [JsonIgnore]
    public override PageType Type { get { return PageType.Standard; } }
}