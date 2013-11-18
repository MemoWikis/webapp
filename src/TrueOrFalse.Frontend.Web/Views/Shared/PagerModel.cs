using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seedworks.Lib.Persistence;

public class PagerModel
{
    /// <summary> The minimum number of page buttons to show before and after the current page.</summary>
    private const int Size = 2;

    public int CurrentPage;
    public bool HasPreviousPage;
    public bool HasNextPage;
    public int LastPage;

    /// <summary> True, if page buttons are skipped after the button for first page.</summary>
    public bool SkippedLeft;

    /// <summary> True, if page buttons are skipped before the button for last page.</summary>
    public bool SkippedRight;

    /// <summary> The first page button to show after the button for the first page.</summary>
    public int Start;

    /// <summary> The last page button to show before the button for the last page.</summary>
    public int Stop;

    /// <summary> The number of page buttons except the buttons for the first and last page.</summary>
    public int PageCountWithoutLastAndFirst;

    public string Controller;
    public string Action;

    public PagerModel(IPager pager)
    {
        CurrentPage = pager.CurrentPage;
        HasPreviousPage = pager.HasPreviousPage();
        HasNextPage = pager.HasNextPage();
        LastPage = pager.PageCount;
        Start = Math.Max(CurrentPage - Size, 2);
        SkippedLeft = (Start > 2);
        Stop = Math.Min(CurrentPage + Size, LastPage - 1);
        SkippedRight = (Stop < LastPage - 1);
        PageCountWithoutLastAndFirst = Stop - Start + 1;

        Controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
        Action = HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
    }

}
