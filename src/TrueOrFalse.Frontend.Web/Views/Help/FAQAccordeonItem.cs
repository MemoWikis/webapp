using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using TrueOrFalse.Frontend.Web.Code;


public class FAQAccordeonItem
{
    public string Name;
    public string ItemHtmlIdHeading;
    public string ItemHtmlIdText;
    public string UrlToSection;
    public string HashOfSection;

    public FAQAccordeonItem(string name)
    {
        Name = name;
        ItemHtmlIdHeading = "FaqHeading" + name;
        ItemHtmlIdText = "FaqText" + name;
        UrlToSection = Links.FAQItem(name);
        HashOfSection = "#" + name;
    }
}
