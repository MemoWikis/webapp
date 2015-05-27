public class OverwrittenConfigValueResult
{
    public OverwrittenConfigValueResult(bool hasValue, string value)
    {
        HasValue = hasValue;
        Value = value;
    }

    public bool HasValue;
    public string Value;
}