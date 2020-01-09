using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Xml.Linq;
using Seedworks.Web.State;

public static class OverwrittenConfig
{
    public static bool UseWebConfig { get { return Settings.UseWebConfig; } }

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
        string filePath;
        if (ContextUtil.IsWebContext)
            filePath = HttpContext.Current.Server.MapPath(@"~/Web.overwritten.config");
        else if(UseWebConfig)
            filePath = Path.Combine(new DirectoryInfo(AssemblyDirectory).Parent.FullName, "Web.overwritten.config");
        else
            filePath = Path.Combine(new DirectoryInfo(AssemblyDirectory).Parent.Parent.FullName, "App.overwritten.config");

        if (!File.Exists(filePath))
            return new OverwrittenConfigValueResult(false, null);

        var xDoc = XDocument.Load(filePath);
            
        if(xDoc.Root.Element(itemName) == null)
            return new OverwrittenConfigValueResult(false, null);

        var value = xDoc.Root.Element(itemName).Value;

        return new OverwrittenConfigValueResult(true, value);
    }

    static private string AssemblyDirectory
    {
        get
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}