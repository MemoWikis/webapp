using System;
using Newtonsoft.Json;

[Serializable]
public abstract class CategoryBase<T> : ICategoryBase
{
    public abstract CategoryType Type { get; }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static T FromJson(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}