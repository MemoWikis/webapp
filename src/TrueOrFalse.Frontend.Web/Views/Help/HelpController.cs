﻿using System.Web.Mvc;

public class HelpController : BaseController
{
    [SetMenu(MenuEntry.Help)]
    public ActionResult FAQ() { return View(new HelpModel()); }

    [SetMenu(MenuEntry.Help)]
    public ActionResult Widget() { return View(new BaseModel()); }

    [SetMenu(MenuEntry.Help)]
    public ActionResult WidgetPricing() { return View(new BaseModel()); }
}
