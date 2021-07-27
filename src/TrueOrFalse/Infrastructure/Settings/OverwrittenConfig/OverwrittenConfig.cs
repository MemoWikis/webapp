using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Seedworks.Web.State;

public static class OverwrittenConfig
{
    private static readonly Dictionary<string, string> _stringValues = new Dictionary<string, string>();
    private static readonly Dictionary<string, bool> _booleanValues = new Dictionary<string, bool>();
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
            string filePath = ContextUtil.GetFilePath(
                ContextUtil.IsWebContext || ContextUtil.UseWebConfig ? "Web.overwritten.config" : "App.overwritten.config"
            );

            if (!File.Exists(filePath))
                return new OverwrittenConfigValueResult(false, null);

            _xDoc = XDocument.Load(filePath);
        }

            
        if(_xDoc.Root.Element(itemName) == null)
            return new OverwrittenConfigValueResult(false, null);

        var value = _xDoc.Root.Element(itemName).Value;

        return new OverwrittenConfigValueResult(true, value);
    }
}