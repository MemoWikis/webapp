using Newtonsoft.Json;

[Serializable]
public class PageTypeCourse : PageTypeBase<PageTypeCourse>
{

    [JsonIgnore]
    public override PageType Type => PageType.Course;
}