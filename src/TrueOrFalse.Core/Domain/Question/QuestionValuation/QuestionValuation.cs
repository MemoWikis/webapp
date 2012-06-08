using System;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class QuestionValuation : IPersistable, WithDateCreated
    {
        public virtual int Id { get; set; }

        public virtual int UserId { get; set; }
        public virtual int QuestionId { get; set; }

        public virtual int Quality { get; set; }
        public virtual int RelevancePesonal { get; set; }
        public virtual int RelevanceForAll { get; set; }

        public virtual DateTime DateCreated { get; set;  }
    }
}
