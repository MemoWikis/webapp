using Newtonsoft.Json;

[Serializable]
public class PageTypeStandard : PageTypeBase<PageTypeStandard>
{
    [JsonIgnore]
    public override PageType Type { get { return PageType.Standard; } }
}