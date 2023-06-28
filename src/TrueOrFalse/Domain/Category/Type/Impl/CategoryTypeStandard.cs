using Newtonsoft.Json;

[Serializable]
public class CategoryTypeStandard : CategoryTypeBase<CategoryTypeStandard>
{
    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.Standard; } }
}