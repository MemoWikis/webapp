using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;

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
                                Eigentlich gibt es hier nichts zu sehen, 
                                wenn du nicht angemeldet ist.
                            </p>
                            <p>Wir wünschen dir weiter viel Spaß beim Stöbern.</p>
                            <p>
                                Viele Grüße<br>
                                Jule & Robert
                            </p>",
                    DateCreated = DateTime.Now,
                    IsRead = false
                }
            }));

        var messages = Resolve<MessageRepository>()
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

}