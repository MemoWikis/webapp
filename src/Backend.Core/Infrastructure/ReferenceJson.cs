using Newtonsoft.Json;

public class ReferenceJson
{
    public int PageId;
    public int ReferenceId;
    public string ReferenceType;
    public string AdditionalText;
    public string ReferenceText;

    public static IList<Reference> LoadFromJson(
        string json,
        Question question,
        PageRepository pageRepository)
    {
        var referencesJson = JsonConvert.DeserializeObject<IEnumerable<ReferenceJson>>(json);

        return referencesJson.Select(refJson =>
        {
            return new Reference
            {
                Id = refJson.ReferenceId == -1 ? default(int) : refJson.ReferenceId,
                ReferenceType = Reference.GetReferenceType(refJson.ReferenceType),
                Question = question,
                Page = pageRepository.GetByIdEager(refJson.PageId),
                AdditionalInfo = refJson.AdditionalText,
                ReferenceText = refJson.ReferenceText
            };
        }).ToList();
    }
}