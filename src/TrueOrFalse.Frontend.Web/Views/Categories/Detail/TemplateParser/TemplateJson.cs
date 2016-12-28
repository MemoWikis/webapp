using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

[Serializable]
[DebuggerDisplay("TemplateName: {TemplateName}")]
public class TemplateJson
{
    public string TemplateName;

    public int CategoryId;

    public int SetId;

    public string CssClasses;
}
