using System.Diagnostics;
using static System.String;

[DebuggerDisplay("Raw={Raw}")]
public class Template
{
    public string TemplateName;
    public string Raw;
    public List<Parameter> Parameters;

    public bool IsSet => !IsNullOrEmpty(Raw);

    public Template(string text, string templateName)
    {
        TemplateName = templateName;
        Raw = text;
        Parameters = ParseTemplateParameters.Run(text);
    }

    public Parameter ParamByKey(string key) => Parameters.ByKey(key);
}