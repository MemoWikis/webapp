using Newtonsoft.Json;

[Serializable]
public class PageTypeMovie : PageTypeBase<PageTypeMovie>
{
    [JsonIgnore]
    public override PageType Type => PageType.Movie;
}