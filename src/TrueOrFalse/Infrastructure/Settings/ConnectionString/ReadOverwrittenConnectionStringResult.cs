namespace TrueOrFalse.Infrastructure
{
    public class ReadOverwrittenConnectionStringResult
    {
        public ReadOverwrittenConnectionStringResult(bool hasValue, string value)
        {
            HasValue = hasValue;
            Value = value;
        }

        public bool HasValue;
        public string Value;
    }
}