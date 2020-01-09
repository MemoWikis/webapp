using Newtonsoft.Json;

public class SolutionMetadataText : SolutionMetadata
{
    [JsonProperty("IsCaseSensitive")]
    public bool IsCaseSensitive;

    [JsonProperty("IsExtracInput")]
    public bool IsExactInput;

    public SolutionMetadataText()
    {
        IsText = true;
    }

    protected override void InitFromJson(string json)
    {
        var tmp = JsonConvert.DeserializeObject<SolutionMetadataText>(json);
        IsCaseSensitive = tmp.IsCaseSensitive;
        IsExactInput = tmp.IsExactInput;
    }
}