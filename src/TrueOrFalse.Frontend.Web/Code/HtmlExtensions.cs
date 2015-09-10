﻿using System;
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

    public static string IfTrue(this HtmlHelper helper, bool show, string text)
    {
        return show ? text : "";
    }

    public static string Plural(this HtmlHelper helper, int amount, string pluralSuffix, string singularSuffix = "", string zeroSuffix = "")
    {
        if (amount > 1)
            return pluralSuffix;
        return amount == 0 ? zeroSuffix : singularSuffix;
    }
}
