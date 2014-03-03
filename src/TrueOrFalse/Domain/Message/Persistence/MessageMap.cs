using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
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

            Map(x => x.DateCreated);
            Map(x => x.DateModified);
        }
    }
}
