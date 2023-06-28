using Newtonsoft.Json;

[Serializable]
public class CategoryTypeFieldOfTraining : CategoryTypeBase<CategoryTypeFieldOfTraining>
{

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.FieldOfTraining; } }
}