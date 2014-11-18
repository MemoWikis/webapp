using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[Serializable]
public class ParseImageNotifications
{
    public List<Notification> Author = new List<Notification>();
    public List<Notification> Description = new List<Notification>();

    public string ToJson()
    {
        var json = JsonConvert.SerializeObject(this);

        JObject jObject = JObject.Parse(json);
        var itemsToRemove = new List<string> { };
        foreach (var item in jObject)
        {
            if (!item.Value.HasValues)
                itemsToRemove.Add(item.Key);
        }

        foreach (var key in itemsToRemove)
        {
            jObject.Remove(key);
        }

        if (jObject.Count == 0)
            return "";

        return jObject.ToString();
    }

    public static ParseImageNotifications FromJson(string json)
    {
        if (json == null)
            json = "";
        
        return JsonConvert.DeserializeObject<ParseImageNotifications>(json) ?? new ParseImageNotifications();
    }

}

[Serializable]
public class Notification
{
    public string Name;
    public string NotificationText;
}
