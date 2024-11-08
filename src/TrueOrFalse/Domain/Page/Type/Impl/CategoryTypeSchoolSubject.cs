using Newtonsoft.Json;

[Serializable]
public class CategoryTypeSchoolSubject : CategoryTypeBase<CategoryTypeSchoolSubject>
{
    [JsonIgnore]
    public override PageType Type { get { return PageType.SchoolSubject; } }
}