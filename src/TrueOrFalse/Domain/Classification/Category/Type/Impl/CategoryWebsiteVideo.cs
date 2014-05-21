using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class CategoryWebsiteVideo : ICategoryType
{
    public string Url;

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static CategoryWebsiteVideo FromJson(string json)
    {
        return JsonConvert.DeserializeObject<CategoryWebsiteVideo>(json);
    }
}