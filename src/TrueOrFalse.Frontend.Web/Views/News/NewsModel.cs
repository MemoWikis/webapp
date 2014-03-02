using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Mapping;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;

public class NewsModel : BaseModel
{
    public bool IsLoggedIn = true;
    public List<NewsModelRow> Rows = new List<NewsModelRow>(); 

    public NewsModel(){}

    public NewsModel(IList<Message> messages)
    {
        foreach (var msg in messages)
            Rows.Add(new NewsModelRow(msg));
    }
}

public class NewsModelRow
{
    public int MessageId;
    public bool IsRead;
    public string Subject;
    public string Body;
    public string When;

    public NewsModelRow(Message message)
    {
        MessageId = message.Id;
        IsRead = message.IsRead;
        Subject = message.Subject;
        Body = message.Body;
        When = message.DateCreated.ToString("F");
    }
}