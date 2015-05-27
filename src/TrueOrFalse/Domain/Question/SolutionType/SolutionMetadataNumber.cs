using Newtonsoft.Json;

public class SolutionMetadataNumber : SolutionMetadata
{
    [JsonProperty("Currency")]
    public string Currency;

    public SolutionMetadataNumber()
    {
        IsNumber = true;
    }

    protected override void InitFromJson(string json)
    {
    }
}