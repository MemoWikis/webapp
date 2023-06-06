using Newtonsoft.Json;

[Serializable]
public class ManualImageData
{
    public string AuthorManuallyAdded;
    public string DescriptionManuallyAdded;
    public ManualImageEvaluation ManualImageEvaluation;
    public string ManualRemarks;

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static ManualImageData FromJson(string json)
    {
        return JsonConvert.DeserializeObject<ManualImageData>(json ?? "") ?? new ManualImageData();
    }
}
