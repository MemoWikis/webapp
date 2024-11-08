using Newtonsoft.Json;

[Serializable]
public class CategoryTypeDaily : CategoryTypeBase<CategoryTypeDaily>
{
    public string Title;

    [JsonIgnore]
    public override PageType Type { get { return PageType.Daily; } }
}

