using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class KnowledgeHistoryItem : DomainEntity
    {
        public virtual decimal AmountActiveKnowledge { get; set; }
        public virtual decimal AmountInactiveKnowledge { get; set; }
        public virtual decimal AmountNoDataKnowledge { get; set; }
    }
}
