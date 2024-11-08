using Newtonsoft.Json;

[Serializable]
public class CategoryTypeCourse : CategoryTypeBase<CategoryTypeCourse>
{

    [JsonIgnore]
    public override PageType Type => PageType.Course;
}