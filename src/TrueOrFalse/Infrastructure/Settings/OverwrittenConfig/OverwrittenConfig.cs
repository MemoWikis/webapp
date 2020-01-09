using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Xml.Linq;
using Seedworks.Web.State;

public static class OverwrittenConfig
{
    

    public static string ValueString(string itemName)
    {
        var result = Value(itemName);
        return !result.HasValue ? "" : result.Value;        
    }

    public static bool ValueBool(string itemName)
    {
        var result = Value(itemName);
        return result.HasValue && Convert.ToBoolean(result.Value);
    }

    public static OverwrittenConfigValueResult Value(string itemName)
    {
        string filePath = ContextUtil.GetFilePath(
            ContextUtil.IsWebContext || ContextUtil.UseWebConfig ? "Web.overwritten.config" : "App.overwritten.config"
        );

        if (!File.Exists(filePath))
            return new OverwrittenConfigValueResult(false, null);

        var xDoc = XDocument.Load(filePath);
            
        if(xDoc.Root.Element(itemName) == null)
            return new OverwrittenConfigValueResult(false, null);

        var value = xDoc.Root.Element(itemName).Value;

        return new OverwrittenConfigValueResult(true, value);
    }
}