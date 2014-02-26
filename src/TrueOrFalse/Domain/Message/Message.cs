using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    [DebuggerDisplay("Id={Id} Title={Text}")]
    public class Message
    {
        public virtual string Subject { get; set; }
        public virtual string Body { get; set;  }
        //public virtual 
    }
}
