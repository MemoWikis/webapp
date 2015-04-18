using System.Collections.Generic;
using System.Globalization;
using TrueOrFalse;

public class MessageModel : BaseModel
{
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
        WhenDatetime = message.DateCreated.ToString("", new CultureInfo("de-DE"));
    }
}