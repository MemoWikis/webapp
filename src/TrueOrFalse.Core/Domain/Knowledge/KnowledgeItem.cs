using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class KnowledgeItem : DomainEntity
    {
        public virtual int QuestionId { get; set;  }
        public virtual int UserId { get; set; }

        public virtual decimal CorrectnessProbability { get; set; }
    }
}
