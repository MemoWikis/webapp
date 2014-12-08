using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrueOrFalse;

[Serializable]
public class ManualImageData
{
    public string AuthorManuallyAdded;
    public string DescriptionManuallyAdded;
    public ManualImageEvaluation ManualImageEvaluation;
    public string ManualNotification;

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static ManualImageData FromJson(string json)
    {
        return JsonConvert.DeserializeObject<ManualImageData>(json ?? "") ?? new ManualImageData();
    }
}
