using Newtonsoft.Json;

[Serializable]
public abstract class PageTypeBase<T> : IPageTypeBase where T : new()
{
    [JsonIgnore]
    public Page Page { get; set; }

    public abstract PageType Type { get; }

    public static T FromJson(Page page)
    {
        if (page.TypeJson == null)
            return new T();

        var result = JsonConvert.DeserializeObject<T>(page.TypeJson);
        ((IPageTypeBase)result).Page = page;
        return result;
    }
}