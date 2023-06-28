using System.Collections.Generic;
using System.Linq;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Infrastructure
{
    public class DbSettings : DomainEntity
    {
        public virtual int AppVersion { get; set; }
        public virtual string SuggestedGames { get; set; }
        public virtual string SuggestedSetsIdString { get; set; }
    }
}