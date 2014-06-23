using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

[Serializable]
public class CategoryTypeDailyArticle : CategoryTypeBase<CategoryTypeDailyArticle>
{
    public string Title;
    public string Subtitle;
    public string Author;
    public string Url;

     [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.DailyArticle; } }
}

