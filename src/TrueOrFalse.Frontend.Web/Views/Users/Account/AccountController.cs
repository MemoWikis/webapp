﻿using System.Web.Mvc;

public class AccountController : BaseController
{
    [AccessOnlyAsAdmin]
    public ActionResult RemoveAdminRights()
    {
        SessionUserLegacy.IsInstallationAdmin = false;

        if (Request.UrlReferrer == null)
        {
            Redirect("/");
        }

        return Redirect(Request.UrlReferrer.AbsolutePath);
    }
}