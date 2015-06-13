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
        var tmp = JsonConvert.DeserializeObjectAsync<SolutionMetadataText>(json);
        IsCaseSensitive = tmp.Result.IsCaseSensitive;
        IsExactInput = tmp.Result.IsExactInput;
    }
}