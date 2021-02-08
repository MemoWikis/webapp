using FluentNHibernate.Mapping;

public class MessageMap : ClassMap<Message>
{
    public MessageMap()
    {
        Id(x => x.Id);
        Map(x => x.ReceiverId);
        Map(x => x.Subject);
        Map(x => x.Body);
        Map(x => x.MessageType);
        Map(x => x.IsRead);

        Map(x => x.WasSendPerEmail);
        Map(x => x.WasSendToSmartphone);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}