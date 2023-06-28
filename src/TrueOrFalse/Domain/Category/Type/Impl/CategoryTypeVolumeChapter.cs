using Newtonsoft.Json;

[Serializable]
public class CategoryTypeVolumeChapter : CategoryTypeBase<CategoryTypeVolumeChapter>
{
    public string Title;
    public string Author;

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.VolumeChapter; } }
}