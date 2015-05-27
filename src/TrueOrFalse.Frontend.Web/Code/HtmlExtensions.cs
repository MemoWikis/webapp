using System.Web.Mvc;

public static class HtmlExtensions
{
    /// <summary>Returns display none if true</summary>
    public static string CssHide(this HtmlHelper helper, bool hide)
    {
        return hide ? "display:none;" : "";
    }
}
