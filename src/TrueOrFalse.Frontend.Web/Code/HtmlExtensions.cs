using System;
using System.Collections;
using System.Linq;
using System.Web.Mvc;
using NHibernate.Util;

public static class HtmlExtensions
{
    /// <summary>Returns display none if true</summary>
    public static string CssHide(this HtmlHelper helper, bool hide)
    {
        return hide ? "display:none;" : "";
    }

    public static string Plural(this HtmlHelper helper, int amount, string pluralSuffix)
    {
        if (amount > 1)
            return pluralSuffix;

        return "";
    }
}
