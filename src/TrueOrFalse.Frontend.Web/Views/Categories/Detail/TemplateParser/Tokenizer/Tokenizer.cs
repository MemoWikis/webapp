using System.Collections.Generic;
using Newtonsoft.Json;

public static class Tokenizer
{
    public static List<TemplateJson> Run(string contentString)
    {
        dynamic jsonObject = JsonConvert.DeserializeObject(contentString);
        var tokens = new List<TemplateJson>();

        foreach (var obj in jsonObject)
        {
            var json = new TemplateJson{
                TemplateName = obj.TemplateName
            };
            json.OriginalJson = obj.ToString();
            tokens.Add(json);
        }

        return tokens;
    }
}