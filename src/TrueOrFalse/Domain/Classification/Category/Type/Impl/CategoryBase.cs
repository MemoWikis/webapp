using Newtonsoft.Json;

public class CategoryBase<T>
{
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static T FromJson(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}