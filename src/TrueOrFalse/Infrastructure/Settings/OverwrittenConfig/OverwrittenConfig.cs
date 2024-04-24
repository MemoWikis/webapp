using System.Xml.Linq;

public class OverwrittenConfig
{
    private static readonly Dictionary<string, string> _stringValues = new();
    private static Dictionary<string, bool> _booleanValues = new();
    private static XDocument _xDoc;

    public static string ValueString(string itemName)
    {
        if (_stringValues.ContainsKey(itemName))
            return _stringValues[itemName];

        var result = Value(itemName);
        var resultValue = !result.HasValue ? "" : result.Value;
        _stringValues.Add(itemName, resultValue);

        return resultValue;
    }

    public static bool ValueBool(string itemName)
    {
        if (_booleanValues.ContainsKey(itemName))
            return _booleanValues[itemName];

        var result = Value(itemName);
        var resultValue = result.HasValue && Convert.ToBoolean(result.Value);
        _booleanValues.Add(itemName, resultValue);

        return resultValue;
    }

    public static OverwrittenConfigValueResult Value(string itemName)
    {
        if (_xDoc == null)
        {
            string filePath = Path.Combine(AppContext.BaseDirectory, "Web.overwritten.config");

            if (!File.Exists(filePath))
                return new OverwrittenConfigValueResult(false, null);

            _xDoc = XDocument.Load(filePath);
        }

        if (_xDoc.Root.Element(itemName) == null)
            return new OverwrittenConfigValueResult(false, null);

        var value = _xDoc.Root.Element(itemName).Value;

        return new OverwrittenConfigValueResult(true, value);
    }
}