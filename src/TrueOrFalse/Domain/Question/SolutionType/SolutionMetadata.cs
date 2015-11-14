using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class SolutionMetadata
{
    private string _orginalJson;
    public string Json
    {
        get { return JsonConvert.SerializeObject(this); }
        set
        {
            _orginalJson = value;
            InitFromJson(value);
        }
    }

    [JsonProperty("IsDate")]
    public bool IsDate;
    [JsonProperty("IsNumber")]
    public bool IsNumber;
    [JsonProperty("IsText")]
    public bool IsText;

    protected virtual void InitFromJson(string json)
    {
        if (json == null)
            return;

        var tmp = JsonConvert.DeserializeObjectAsync<SolutionMetadata>(json);

        IsDate = tmp.Result.IsDate;
        IsNumber = tmp.Result.IsNumber;
        IsText = tmp.Result.IsText;
    }

    public SolutionMetadataDate GetForDate(){
        return new SolutionMetadataDate { Json = _orginalJson };
    }

    public SolutionMetadataNumber GetForNumber(){
        return new SolutionMetadataNumber { Json = _orginalJson };
    }

    public SolutionMetadataText GetForText(){
        return new SolutionMetadataText { Json = _orginalJson };
    }
}