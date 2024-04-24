using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[Serializable]
public class ImageParsingNotifications
{
    public List<Notification> InfoTemplate = new();
    public List<Notification> Author = new();
    public List<Notification> Description = new();

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

    public static ImageParsingNotifications FromJson(string json)
    {
        return JsonConvert.DeserializeObject<ImageParsingNotifications>(json ?? "") ??
               new ImageParsingNotifications();
    }
}

[Serializable]
public class Notification
{
    public string Name;
    public string NotificationText;
}