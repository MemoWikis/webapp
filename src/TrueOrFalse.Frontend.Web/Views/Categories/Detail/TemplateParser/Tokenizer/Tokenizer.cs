using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHibernate.Exceptions;

public static class Tokenizer
{
    public static List<TemplateJson> Run(string contentString)
    {
        dynamic jsonObject = JsonConvert.DeserializeObject(contentString);

        var tokens = new List<TemplateJson>();
        TemplateJson loadbeforeLastLoadToken = null; 
        TemplateJson loadAsLastToken = null; 


        foreach (var obj in jsonObject)
        {

            var json = AddJsonTemplate(obj);

            if (obj.Load != null && obj.Load.Value.Equals("All"))
            {
                var tempObj =
                    "[{\"TemplateName\":\"InlineText\",\"Content\":\"<h3 id=\\\"AllSubtopics\\\">Alle Unterthemen</h3>\\n\"}]";
                loadbeforeLastLoadToken = AddJsonTemplate(JsonConvert.DeserializeObject(tempObj), true); 
                loadAsLastToken = json;
            }
               
            else
                tokens.Add(json);
        }
        if(loadAsLastToken != null)
            tokens.Add(loadbeforeLastLoadToken);
            tokens.Add(loadAsLastToken);

        return tokens;
    }

    private static TemplateJson AddJsonTemplate(dynamic obj, bool isBeforeLast = false)
    {
        var json = new TemplateJson
        {
            TemplateName = isBeforeLast ? obj.First.TemplateName :  obj.TemplateName
        };
        json.OriginalJson = isBeforeLast ? obj.First.ToString() :  obj.ToString();

        return json;
    }
}