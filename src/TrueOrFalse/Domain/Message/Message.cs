using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    [DebuggerDisplay("Id={Id} Title={Text}")]
    public class Message : DomainEntity
    {
        public virtual int ReceiverId { get; set; }
        public virtual string Subject { get; set; }
        public virtual string Body { get; set;  }

        public virtual string MessageType { get; set; }
        
        public virtual bool IsRead { get; set; }
    }
}
