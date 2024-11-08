using Newtonsoft.Json;

[Serializable]
public class CategoryTypeMovie : CategoryTypeBase<CategoryTypeMovie>
{
    [JsonIgnore]
    public override PageType Type { get { return PageType.Movie; } }
}