using Newtonsoft.Json;

[Serializable]
public class CategoryTypeBook : CategoryTypeBase<CategoryTypeBook>
{
    public string Title;
    public string Author;

    [JsonIgnore]
    public override PageType Type{ get { return PageType.Book; } }
}