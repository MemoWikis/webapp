using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core.Infrastructure
{
    public class DbSettings : DomainEntity
    {
        public virtual int AppVersion { get; set; }
    }
}