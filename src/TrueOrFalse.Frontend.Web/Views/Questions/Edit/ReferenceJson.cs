using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using TrueOrFalse;


public class ReferenceJson
{
    public int CategoryId;
    public int ReferenceId;
    public string AdditionalText;
    public string FreeText;

    public static IList<Reference> LoadFromJson(string json, Question question)
    {
        var referencesJson= JsonConvert.DeserializeObject<IEnumerable<ReferenceJson>>(json);

        var catRepo = Sl.Resolve<CategoryRepository>();

        return referencesJson.Select(refJson =>
        {
            return new Reference
            {
                Id = refJson.ReferenceId,
                Question = question,
                Category = catRepo.GetById(refJson.CategoryId),
                AdditionalInfo = refJson.AdditionalText,
                FreeTextReference = refJson.FreeText
            };
        }).ToList();
    }

}