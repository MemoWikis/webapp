using Newtonsoft.Json;

[Serializable]
public class PageTypeMagazine : PageTypeBase<PageTypeMagazine>
{
    public string Title;

    [JsonIgnore]
    public override PageType Type
    {
        get { return PageType.Magazine; }
    }
}