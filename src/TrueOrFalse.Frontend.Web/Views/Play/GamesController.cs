using System;
using System.Collections.Generic;
using System.Web.Mvc;

public class GamesController : BaseController
{
    [SetMenu(MenuEntry.Play)]
    public ActionResult Games()
    {
        return View(new GamesModel());
    }
}