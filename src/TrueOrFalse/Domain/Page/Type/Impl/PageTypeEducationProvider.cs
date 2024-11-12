using Newtonsoft.Json;

[Serializable]
public class PageTypeEducationProvider : PageTypeBase<PageTypeEducationProvider>
{

    [JsonIgnore]
    public override PageType Type => PageType.EducationProvider;
}