using System;
using System.Diagnostics;
using Newtonsoft.Json;

[Serializable]
[DebuggerDisplay("TemplateName: {TemplateName}")]
public class TemplateJson
{
    public string TemplateName;
    public string OriginalJson = "";

    [JsonIgnore]
    public string InlineText = "";
}
