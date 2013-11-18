using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    /// <summary>
    /// Relation between QuestionSet -> Question
    /// Contains order
    /// </summary>
    public class QuestionInSet : DomainEntity
    {
        public virtual Set Set { get; set; }
        public virtual Question Question { get; set; }
        public virtual int Sort { get; set; }

    }
}
