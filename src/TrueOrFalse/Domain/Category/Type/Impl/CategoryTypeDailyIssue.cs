using System.Linq;
using Newtonsoft.Json;

[Serializable]
public class CategoryTypeDailyIssue : CategoryTypeBase<CategoryTypeDailyIssue>
{
    public string No;

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.DailyIssue; } }
}

