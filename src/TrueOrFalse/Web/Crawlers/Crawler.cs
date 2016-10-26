using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class Crawler
{
    [JsonProperty("pattern")]
    public string Pattern;
}

