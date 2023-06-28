using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrueOrFalse;

[Serializable]
public class CategoryTypeMagazineArticle : CategoryTypeBase<CategoryTypeMagazineArticle>
{
    public string Title;
    public string Author;

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.MagazineArticle; } }
}

