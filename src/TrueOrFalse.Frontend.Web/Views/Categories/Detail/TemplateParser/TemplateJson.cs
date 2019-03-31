using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

[Serializable]
[DebuggerDisplay("TemplateName: {TemplateName}")]
public class TemplateJson
{
    public string TemplateName;
    public string OriginalJson;
}
