using Newtonsoft.Json;

[Serializable]
public class CategoryTypeMagazineArticle : CategoryTypeBase<CategoryTypeMagazineArticle>
{
    public string Title;
    public string Author;

    [JsonIgnore]
    public override CategoryType Type
    {
        get { return CategoryType.MagazineArticle; }
    }
}