using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;

public class NewsController : BaseController
{
    [SetMenu(MenuEntry.News)]
    public ActionResult News()
    {
        if (!_sessionUser.IsLoggedIn)
            return View(new NewsModel{IsLoggedIn = false});

        var messages = Resolve<MessageRepository>()
            .GetForUser(_sessionUser.User.Id);

        return View(new NewsModel(messages));
    }
}