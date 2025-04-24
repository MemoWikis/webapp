using Newtonsoft.Json;

[Serializable]
public class PageTypeDailyIssue : PageTypeBase<PageTypeDailyIssue>
{
    public string No;

    [JsonIgnore]
    public override PageType Type
    {
        get { return PageType.DailyIssue; }
    }
}