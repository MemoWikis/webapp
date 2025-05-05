
using Newtonsoft.Json;

[Serializable]
public class PageTypeFieldOfStudy : PageTypeBase<PageTypeFieldOfStudy>
{

    [JsonIgnore]
    public override PageType Type { get { return PageType.FieldOfStudy; } }
}