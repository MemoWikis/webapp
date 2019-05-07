using System.Collections.Generic;

public class BaseContentModule : BaseModel
{
    public string Markdown;
    public string Type;

    public bool IsText => Type == "inlinetext";

    public TemplateJson TemplateJson;
}