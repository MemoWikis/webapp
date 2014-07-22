using System;
using Newtonsoft.Json;
using TrueOrFalse;

[Serializable]
public abstract class CategoryTypeBase<T> : ICategoryTypeBase
{
    [JsonIgnore]
    public Category Category { get; set; }

    public abstract CategoryType Type { get; }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static T FromJson(Category category)
    {
        var result = JsonConvert.DeserializeObject<T>(category.TypeJson);
        ((ICategoryTypeBase)result).Category = category;
        return result;
    }
}