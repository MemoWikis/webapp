using Newtonsoft.Json;

[Serializable]
public class CategoryTypeDailyIssue : CategoryTypeBase<CategoryTypeDailyIssue>
{
    public string No;

    [JsonIgnore]
    public override PageType Type
    {
        get { return PageType.DailyIssue; }
    }
}