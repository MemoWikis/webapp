using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

[Serializable]
public class CategoryTypeDaily : CategoryTypeBase<CategoryTypeDaily>
{
    public string Title;
    public string ISSN;
    public string Publisher;

     [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.Daily; } }
}

