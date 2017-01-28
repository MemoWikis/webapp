using FluentNHibernate.Mapping;

public class MessageEmailMap : ClassMap<MessageEmail>
{
    public MessageEmailMap()
    {
        Id(x => x.Id);
        References(x => x.User);
        Map(x => x.MessageEmailType);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}