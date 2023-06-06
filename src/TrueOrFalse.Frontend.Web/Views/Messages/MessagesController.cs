using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class MessagesController : BaseController
{
    public MessagesController(SessionUser sessionUser) : base(sessionUser)
    {
        
    }

    [SetUserMenu(UserMenuEntry.Messages)]

    public ActionResult Messages()
    {
        return View(new MessageModel());
    }

    public string RenderAllMessagesInclRead()
    {
        if (!_sessionUser.IsLoggedIn)
        {
            return "";
        }

        var messages = Resolve<MessageRepo>()
            .GetForUser(_sessionUser.UserId, false)
            .Select(m => new MessageModelRow(m))
            .ToList();

        return ViewRenderer.RenderPartialView("~/Views/Messages/Partials/MessagesRows.ascx", messages, ControllerContext);
    }

    [HttpPost]
    public EmptyResult SetMessageRead(int msgId){
        Resolve<SetMessageRead>().Run(msgId); return  new EmptyResult();
    }

    [HttpPost]
    public EmptyResult SetMessageUnread(int msgId){
        Resolve<SetMessageUnread>().Run(msgId); return new EmptyResult();
    }

    public ActionResult GameInfo()
    {
        return Content(ViewRenderer.RenderPartialView(
            "MessageGame", new MessageModel(), ControllerContext
        ));
    }

}