using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;

public class HelpController : BaseController
{
    [SetMenu(MenuEntry.Help)]
    public ActionResult Willkommen(){ AddActionToHistory(); return View(new HelpModel()); }
    [SetMenu(MenuEntry.Help)]
    public ActionResult DatenSicherheit() { AddActionToHistory(); return View(new HelpModel()); }
    [SetMenu(MenuEntry.Help)]
    public ActionResult Reputation() { AddActionToHistory(); return View(new HelpModel()); }
    [SetMenu(MenuEntry.Help)]
    public ActionResult Wissen() { AddActionToHistory(); return View(new HelpModel()); }
    [SetMenu(MenuEntry.Help)]
    public ActionResult UrheberrechtInhalte() { AddActionToHistory(); return View(new HelpModel()); }

    public void AddActionToHistory(){
        _sessionUiData.VisitedHelpPages.Add(new HelpHistoryItem(GetActionName()));
    }

    public string GetActionName(){
        return this.RouteData.Values["action"].ToString();
    }
}
