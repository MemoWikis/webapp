using Newtonsoft.Json;

public enum DatePrecision
{
    Day = 1,
    Month = 2,
    Year = 3,
    Decade = 4,
    Century = 5,
    Millenium = 6
}

public class SolutionMetadataDate : SolutionMetadata
{
    public SolutionMetadataDate(){
        IsDate = true;
    }

    [JsonProperty("Precision")]
    public DatePrecision Precision;

    protected override void InitFromJson(string json)
    {
        var tmp = JsonConvert.DeserializeObject<SolutionMetadataDate>(json);
        Precision = tmp.Precision;
    }
}