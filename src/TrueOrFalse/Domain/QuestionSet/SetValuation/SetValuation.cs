using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class SetValuation : DomainEntity
    {
        public virtual int UserId { get; set; }
        public virtual int SetId { get; set; }

        public virtual int RelevancePersonal { get; set; }
    }
}
