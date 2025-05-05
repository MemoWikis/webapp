using Newtonsoft.Json;

[Serializable]
public class PageTypeFieldOfTraining : PageTypeBase<PageTypeFieldOfTraining>
{

    [JsonIgnore]
    public override PageType Type { get { return PageType.FieldOfTraining; } }
}