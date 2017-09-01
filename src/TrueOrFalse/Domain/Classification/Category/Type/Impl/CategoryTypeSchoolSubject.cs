using System;
using Newtonsoft.Json;

[Serializable]
public class CategoryTypeSchoolSubject : CategoryTypeBase<CategoryTypeSchoolSubject>
{

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.SchoolSubject; } }
}