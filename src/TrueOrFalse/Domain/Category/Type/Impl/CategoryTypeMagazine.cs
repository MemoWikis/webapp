using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

[Serializable]
public class CategoryTypeMagazine : CategoryTypeBase<CategoryTypeMagazine>
{
    public string Title;

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.Magazine; } }
}

