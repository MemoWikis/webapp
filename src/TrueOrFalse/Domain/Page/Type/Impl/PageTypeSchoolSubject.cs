using Newtonsoft.Json;

[Serializable]
public class PageTypeSchoolSubject : PageTypeBase<PageTypeSchoolSubject>
{
    [JsonIgnore]
    public override PageType Type { get { return PageType.SchoolSubject; } }
}