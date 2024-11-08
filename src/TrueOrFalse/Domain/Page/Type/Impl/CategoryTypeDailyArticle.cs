using Newtonsoft.Json;

[Serializable]
public class CategoryTypeDailyArticle : CategoryTypeBase<CategoryTypeDailyArticle>
{
    public string Title;
    public string Author;

    [JsonIgnore]
    public override PageType Type
    {
        get { return PageType.DailyArticle; }
    }
}