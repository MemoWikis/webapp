﻿using System.Web.Mvc;

public class AboutController : Controller
{
    [SetMenu(MenuEntry.None)]
    public ActionResult AboutMemucho()
    {
        return View(new BaseModel());
    }

    [SetMenu(MenuEntry.None)]
    public ActionResult ForTeachers()
    {
        return View(new BaseModel());
    }

    [SetMenu(MenuEntry.None)]
    public ActionResult Jobs()
    {
        return View(new BaseModel());
    }

    [SetMenu(MenuEntry.None)]
    public ActionResult WelfareCompany()
    {
        return View(new BaseModel());
    }


}