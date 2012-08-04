using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class KnowledgeItem : DomainEntity
    {
        public virtual decimal AvgLast3Repetitions { get; set; }
        public virtual decimal AvgLast10Repetitions { get; set; }
        public virtual int TotalRepetitions { get; set; }
        
        public virtual DateTime LastRepetition { get; set; }

        public virtual decimal CorrectnessProbability { get; set; }
        public virtual decimal TimeModificator { get; set; }
    }
}
