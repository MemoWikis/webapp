using Newtonsoft.Json;

[Serializable]
public class PageTypeMagazineIssue : PageTypeBase<PageTypeMagazineIssue>
{
    public string No;
    public string Title;

    [JsonIgnore]
    public override PageType Type
    {
        get { return PageType.MagazineIssue; }
    }
}