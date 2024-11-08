using Newtonsoft.Json;

[Serializable]
public class CategoryTypeEducationProvider : CategoryTypeBase<CategoryTypeEducationProvider>
{

    [JsonIgnore]
    public override PageType Type => PageType.EducationProvider;
}