using Newtonsoft.Json;

[Serializable]
public class CategoryTypeMagazineIssue : CategoryTypeBase<CategoryTypeMagazineIssue>
{
    public string No;
    public string Title;

    [JsonIgnore]
    public override PageType Type
    {
        get { return PageType.MagazineIssue; }
    }
}