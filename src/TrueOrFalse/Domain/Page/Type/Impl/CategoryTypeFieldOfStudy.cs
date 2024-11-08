
using Newtonsoft.Json;

[Serializable]
public class CategoryTypeFieldOfStudy : CategoryTypeBase<CategoryTypeFieldOfStudy>
{

    [JsonIgnore]
    public override PageType Type { get { return PageType.FieldOfStudy; } }
}