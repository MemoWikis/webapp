using Newtonsoft.Json;

public class ReferenceJson
{
    public int CategoryId;
    public int ReferenceId;
    public string ReferenceType;
    public string AdditionalText;
    public string ReferenceText;

    public static IList<Reference> LoadFromJson(
        string json,
        Question question,
        CategoryRepository categoryRepository)
    {
        var referencesJson = JsonConvert.DeserializeObject<IEnumerable<ReferenceJson>>(json);

        return referencesJson.Select(refJson =>
        {
            return new Reference
            {
                Id = refJson.ReferenceId == -1 ? default(int) : refJson.ReferenceId,
                ReferenceType = Reference.GetReferenceType(refJson.ReferenceType),
                Question = question,
                Category = categoryRepository.GetByIdEager(refJson.CategoryId),
                AdditionalInfo = refJson.AdditionalText,
                ReferenceText = refJson.ReferenceText
            };
        }).ToList();
    }
}