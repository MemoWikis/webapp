using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Seedworks.Web.State;

public class OverwrittenConfig
{
    private readonly HttpContext _httpContext;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly Dictionary<string, string> _stringValues = new Dictionary<string, string>();
    private readonly Dictionary<string, bool> _booleanValues = new Dictionary<string, bool>();
    private XDocument _xDoc;

    public OverwrittenConfig(HttpContext httpContext,
        IWebHostEnvironment webHostEnvironment)
    {
        _httpContext = httpContext;
        _webHostEnvironment = webHostEnvironment;
    }
    public string ValueString(string itemName)
    {
        if (_stringValues.ContainsKey(itemName))
            return _stringValues[itemName];

        var result = Value(itemName);
        var resultValue = !result.HasValue ? "" : result.Value;
        _stringValues.Add(itemName, resultValue);

        return resultValue;
    }

    public bool ValueBool(string itemName)
    {
        if (_booleanValues.ContainsKey(itemName))
            return _booleanValues[itemName];

        var result = Value(itemName);
        var resultValue = result.HasValue && Convert.ToBoolean(result.Value);
        _booleanValues.Add(itemName, resultValue);

        return resultValue;
    }

    public OverwrittenConfigValueResult Value(string itemName)
    {
        if (_xDoc == null)
        {
            var contextUtil = new ContextUtil(_httpContext, _webHostEnvironment);
            string filePath = contextUtil.GetFilePath(
              contextUtil.IsWebContext || contextUtil.UseWebConfig ? "Web.overwritten.config" : "App.overwritten.config"
            );

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