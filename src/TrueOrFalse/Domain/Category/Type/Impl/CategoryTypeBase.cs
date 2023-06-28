using Newtonsoft.Json;

[Serializable]
public abstract class CategoryTypeBase<T> : ICategoryTypeBase where T : new()  
{
    [JsonIgnore]
    public Category Category { get; set; }

    public abstract CategoryType Type { get; }

    public static T FromJson(Category category)
    {
        if (category.TypeJson == null)
            return new T();

        var result = JsonConvert.DeserializeObject<T>(category.TypeJson);
        ((ICategoryTypeBase)result).Category = category;
        return result;
    }
}