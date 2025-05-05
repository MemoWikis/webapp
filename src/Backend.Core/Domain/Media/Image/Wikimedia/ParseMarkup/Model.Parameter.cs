using System.Diagnostics;
using static System.String;

[DebuggerDisplay("Raw={Raw}")]
public class Parameter
{
    public string Key;
    public string Value;

    public string Raw;
    public List<string> Tokens;

    public bool HasKey => !IsNullOrEmpty(Key);

    public List<Template> Subtemplates = new List<Template>();

    public Parameter(List<string> currentParameterTokens)
    {
        Raw = currentParameterTokens.Aggregate((a, b) => a + ' ' + b);
        Tokens = currentParameterTokens;

        if (Raw.Contains("="))
        {
            bool isKey = true;
            bool isValue = false;
            foreach (var token in Tokens)
            {
                if (token == "=" && !isValue)
                {
                    isValue = true;
                    isKey = false;
                    continue;
                }

                if (isKey)
                    Key += token;

                if (isValue)
                    Value += token;
            }

            if (Key != null) Key = Key.Trim();
            if (Value != null) Value = Value.Trim();
        }
        else //!Raw.Contains("=")
        {
            Value = Raw;
        }

        Subtemplates = ParseTemplate.GetParameterSubtemplates(this);
    }
}

public static class ParametersExt
{
    public static Parameter ByKey(this IEnumerable<Parameter> parameters, string key)
    {
        parameters = parameters.ToList();

        if (parameters.Where(x => x.HasKey).All(x => x.Key.ToLower() != key.ToLower()))
            return null;

        return parameters.Where(x => x.HasKey).First(x => x.Key.ToLower() == key.ToLower());
    }
}