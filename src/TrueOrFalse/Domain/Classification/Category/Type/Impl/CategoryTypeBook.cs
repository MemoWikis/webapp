using System;
using Newtonsoft.Json;

[Serializable]
public class CategoryTypeBook : CategoryTypeBase<CategoryTypeBook>
{
    public string Title;
    public string Subtitle;
    public string Author;
    public string ISBN;
    public string Publisher;
    public string PublicationCity;
    public string PublicationYear;

     [JsonIgnore]
    public override CategoryType Type{ get { return CategoryType.Book; } }
}