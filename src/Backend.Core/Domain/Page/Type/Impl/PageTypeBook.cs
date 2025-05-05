using Newtonsoft.Json;

[Serializable]
public class PageTypeBook : PageTypeBase<PageTypeBook>
{
    public string Title;
    public string Author;

    [JsonIgnore]
    public override PageType Type{ get { return PageType.Book; } }
}