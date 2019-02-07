using System;
using System.Collections.Generic;
using System.Linq;

public class TextBlockModel : BaseContentModule
{
    public string Text;

    public TextBlockModel(TextBlockJson textBlockJson)
    {
        Text = textBlockJson.Text;
    }
}