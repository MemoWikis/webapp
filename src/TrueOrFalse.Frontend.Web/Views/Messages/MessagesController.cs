using System;
using System.Collections.Generic;
using System.Web.Mvc;

public class MessagesController : BaseController
{
    [SetMenu(MenuEntry.Messages)]
    public ActionResult Messages()
    {
        if (!_sessionUser.IsLoggedIn)
            return View(new MessageModel(new List<Message>()
            {
                new Message
                {
                    Subject = "Hallo Unbekannte(r)!",
                    Body = @"<p>
                                schön, dass du bei uns vorbeischaust. 
                                Du bist nicht eingeloggt, daher gibt es hier eigentlich nichts zu sehen.
                                <a href=" + Url.Action("Login", "Welcome") + @"><i class='fa fa-sign-in'></i>&nbsp;Logge dich am besten gleich ein</a> oder 
                                <a href=" + Url.Action("Register", "Welcome") + @"><i class='fa fa-user-plus'></i>&nbsp;registriere dich</a> als neuer Benutzer, es dauert nur wenige Sekunden.
                            </p>
                            <p>Wir wünschen dir weiter viel Spaß beim Stöbern.</p>
                            <p>
                                Viele Grüße,<br>
                                Christof, Jule & Robert
                            </p>",
                    DateCreated = DateTime.Now,
                    IsRead = false
                }
            }));

        var messages = Resolve<MessageRepo>()
            .GetForUser(_sessionUser.User.Id);

        return View(new MessageModel(messages));
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