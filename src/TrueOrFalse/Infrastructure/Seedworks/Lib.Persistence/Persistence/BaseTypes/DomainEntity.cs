using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seedworks.Lib.Persistence
{
    /// <remarks>
    /// Based on:
    /// https://github.com/Slesa/Playground/blob/master/src/lib/DataAccess/DataAccess/DomainEntity.cs
    /// </remarks>>
    [Serializable]
    public class DomainEntity :  Entity, WithDateCreated, WithDateModified
    {
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime DateModified { get; set; }
    }
}
