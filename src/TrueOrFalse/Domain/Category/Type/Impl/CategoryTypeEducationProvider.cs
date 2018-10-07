using System;
using Newtonsoft.Json;

[Serializable]
public class CategoryTypeEducationProvider : CategoryTypeBase<CategoryTypeEducationProvider>
{

    [JsonIgnore]
    public override CategoryType Type => CategoryType.EducationProvider;
}