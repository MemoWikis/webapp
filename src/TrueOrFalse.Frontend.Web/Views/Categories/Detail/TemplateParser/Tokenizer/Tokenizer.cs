using System.Collections.Generic;
using Newtonsoft.Json;

public static class Tokenizer
{
    public static List<TemplateJson> Run(string contentString)
    {
        var decoded = System.Net.WebUtility.HtmlDecode(contentString);
        dynamic jsonObject = JsonConvert.DeserializeObject(decoded);
        var tokens = new List<TemplateJson>();

        foreach (var obj in jsonObject)
        {
            var json = JsonConvert.DeserializeObject<TemplateJson>(obj.Value);
            json.OriginalJson = obj.Value;
            tokens.Add(json);
        }

        return tokens;
    }
}