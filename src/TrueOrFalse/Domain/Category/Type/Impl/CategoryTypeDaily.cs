using Newtonsoft.Json;

[Serializable]
public class CategoryTypeDaily : CategoryTypeBase<CategoryTypeDaily>
{
    public string Title;

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.Daily; } }
}

