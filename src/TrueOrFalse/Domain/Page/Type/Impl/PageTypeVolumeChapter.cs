using Newtonsoft.Json;

[Serializable]
public class PageTypeVolumeChapter : PageTypeBase<PageTypeVolumeChapter>
{
    public string Title;
    public string Author;

    [JsonIgnore]
    public override PageType Type { get { return PageType.VolumeChapter; } }
}