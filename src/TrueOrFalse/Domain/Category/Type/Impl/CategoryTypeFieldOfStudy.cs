
using Newtonsoft.Json;

[Serializable]
public class CategoryTypeFieldOfStudy : CategoryTypeBase<CategoryTypeFieldOfStudy>
{

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.FieldOfStudy; } }
}