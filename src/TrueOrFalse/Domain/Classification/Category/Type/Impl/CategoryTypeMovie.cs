using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

[Serializable]
public class CategoryTypeMovie : CategoryTypeBase<CategoryTypeMovie>
{
    public string MovieUrl;

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.Movie; } }
}