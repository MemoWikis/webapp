using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrueOrFalse;

[Serializable]
public class CategoryTypeMagazineIssue : CategoryTypeBase<CategoryTypeMagazineIssue>
{
    public string No;
    public string Title;

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.MagazineIssue; } }
}

