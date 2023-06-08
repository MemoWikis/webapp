using System.Linq;
using Newtonsoft.Json;


[Serializable]
public class CategoryTypeDailyArticle : CategoryTypeBase<CategoryTypeDailyArticle>
{
    public string Title;
    public string Author;

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.DailyArticle; } }
}

