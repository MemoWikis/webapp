using Newtonsoft.Json;

[Serializable]
public class CategoryTypeVolumeChapter : CategoryTypeBase<CategoryTypeVolumeChapter>
{
    public string Title;
    public string Author;

    [JsonIgnore]
    public override PageType Type { get { return PageType.VolumeChapter; } }
}