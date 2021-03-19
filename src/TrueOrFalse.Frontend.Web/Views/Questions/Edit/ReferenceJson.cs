using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public class ReferenceJson
{
    public int CategoryId;
    public int ReferenceId;
    public string ReferenceType;
    public string AdditionalText;
    public string ReferenceText;

    public static IList<Reference> LoadFromJson(string json, Question question)
    {
        var referencesJson = JsonConvert.DeserializeObject<IEnumerable<ReferenceJson>>(json);

        var catRepo = Sl.Resolve<CategoryRepository>();

        return referencesJson.Select(refJson =>
        {
            return new Reference
            {
                Id = refJson.ReferenceId == -1 ? default(int) : refJson.ReferenceId,
                ReferenceType = Reference.GetReferenceType(refJson.ReferenceType),
                Question = question,
                Category = EntityCache.GetCategoryCacheItem(refJson.CategoryId),
                AdditionalInfo = refJson.AdditionalText,
                ReferenceText = refJson.ReferenceText
            };
        }).ToList();
    }

}