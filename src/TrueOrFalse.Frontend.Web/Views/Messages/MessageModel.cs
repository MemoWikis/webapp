using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using NHibernate.Mapping;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;

public class MessageModel : BaseModel
{
    public bool IsLoggedIn = true;
    public List<MessageModelRow> Rows = new List<MessageModelRow>();

    public MessageModel()
    {
        
    }

    public MessageModel(IList<Message> messages)
    {
        foreach (var msg in messages)
            Rows.Add(new MessageModelRow(msg));
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
        When = TimeElapsedAsText.Run(message.DateCreated);
        WhenDatetime = message.DateCreated.ToString("F", new CultureInfo("de-DE"));
    }
}