using Newtonsoft.Json;

[Serializable]
public class PageTypeDaily : PageTypeBase<PageTypeDaily>
{
    public string Title;

    [JsonIgnore]
    public override PageType Type { get { return PageType.Daily; } }
}

