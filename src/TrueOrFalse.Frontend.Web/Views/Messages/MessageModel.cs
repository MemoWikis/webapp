using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

public class MessageModel : BaseModel
{
    public List<MessageModelRow> Messages = new List<MessageModelRow>();
    public int ReadMessagesCount;

    public MessageModel()
    {
        if (!_sessionUser.IsLoggedIn)
        {
            Messages.Add(new MessageModelRow(new Message
            {
                Subject = "Hallo Unbekannte(r)!",
                Body = @"<p>
                                schön, dass du bei uns vorbeischaust. 
                                Du bist nicht eingeloggt, daher gibt es hier eigentlich nichts zu sehen.
                                <a href='#' data-btn-login='true'><i class='fa fa-sign-in'></i>&nbsp;Logge dich am besten gleich ein</a> oder 
                                <a href=" + Links.Register() +
                       @"><i class='fa fa-user-plus'></i>&nbsp;registriere dich</a> als neuer Benutzer, es dauert nur wenige Sekunden.
                            </p>
                            <p>Wir wünschen dir weiter viel Spaß beim Stöbern.</p>
                            <p>
                                Viele Grüße,<br>
                                Christof, Jule & Robert
                            </p>",
                DateCreated = DateTime.Now,
                IsRead = false
            }));
        }

        Messages = Resolve<MessageRepo>()
            .GetForUser(_sessionUser.User.Id, true)
            .Select(m => new MessageModelRow(m))
            .ToList();

        ReadMessagesCount = Resolve<MessageRepo>().GetNumberOfReadMessages(_sessionUser.UserId);
    }

}

public class MessageModelRow
{
    public int MessageId;
    public bool IsRead;
    public string Subject;
    public string Body;
    public string When;
    public string WhenDatetime;

    public MessageModelRow(Message message)
    {
        MessageId = message.Id;
        IsRead = message.IsRead;
        Subject = message.Subject;
        Body = message.Body;
        When = DateTimeUtils.TimeElapsedAsText(message.DateCreated);
        WhenDatetime = message.DateCreated.ToString("", new CultureInfo("de-DE"));
    }
}