using Newtonsoft.Json;

[Serializable]
public class CategoryTypeBook : CategoryTypeBase<CategoryTypeBook>
{
    public string Title;
    public string Author;

    [JsonIgnore]
    public override CategoryType Type{ get { return CategoryType.Book; } }
}