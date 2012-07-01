using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public class PersistentLogin
    {
        public virtual int Id { get; set; }
        public virtual int UserId { get; set; }
        public virtual string LoginGuid { get; set; }

        public virtual DateTime Created { get; set; }
    }
}
