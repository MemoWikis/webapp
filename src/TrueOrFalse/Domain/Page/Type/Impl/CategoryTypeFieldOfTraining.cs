using Newtonsoft.Json;

[Serializable]
public class CategoryTypeFieldOfTraining : CategoryTypeBase<CategoryTypeFieldOfTraining>
{

    [JsonIgnore]
    public override PageType Type { get { return PageType.FieldOfTraining; } }
}