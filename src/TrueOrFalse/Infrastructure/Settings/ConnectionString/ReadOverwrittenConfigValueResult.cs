namespace TrueOrFalse.Infrastructure
{
    public class ReadOverwrittenConfigValueResult
    {
        public ReadOverwrittenConfigValueResult(bool hasValue, string value)
        {
            HasValue = hasValue;
            Value = value;
        }

        public bool HasValue;
        public string Value;
    }
}