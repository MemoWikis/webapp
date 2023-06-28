using Newtonsoft.Json;

[Serializable]
public class ManualImageData
{
    public string AuthorManuallyAdded;
    public string DescriptionManuallyAdded;
    public ManualImageEvaluation ManualImageEvaluation;

    public static ManualImageData FromJson(string json)
    {
        return JsonConvert.DeserializeObject<ManualImageData>(json ?? "") ?? new ManualImageData();
    }
}
