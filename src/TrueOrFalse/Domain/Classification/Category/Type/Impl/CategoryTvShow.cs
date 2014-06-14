using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

[Serializable]
public class CategoryTvShow : CategoryBase<CategoryTvShow>
{
    public string Url;

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.TvShow; } }
}