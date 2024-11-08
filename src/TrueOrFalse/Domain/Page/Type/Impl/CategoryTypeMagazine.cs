using Newtonsoft.Json;

[Serializable]
public class CategoryTypeMagazine : CategoryTypeBase<CategoryTypeMagazine>
{
    public string Title;

    [JsonIgnore]
    public override PageType Type
    {
        get { return PageType.Magazine; }
    }
}