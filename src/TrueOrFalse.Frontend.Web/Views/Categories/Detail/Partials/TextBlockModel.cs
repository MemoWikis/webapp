using System;
using System.Collections.Generic;
using System.Linq;

public class TextBlockModel : BaseContentModule
{
    public string Text;

    public TextBlockModel(string text)
    {
        Text = text;
    }
}