using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Seedworks.Lib.Persistence;
using static System.String;

[DebuggerDisplay("Id={Id} Name={Name}")]
[Serializable]
public class Set : DomainEntity, ICreator
{
    public virtual string Name { get; set; }
    public virtual string Text { get; set; }
    public virtual string VideoUrl { get; set; }

    public virtual User Creator { get; set; }

    public virtual int TotalRelevancePersonalAvg { get; set; }
    public virtual int TotalRelevancePersonalEntries { get; set; }

    public virtual Set CopiedFrom { get; set; }
    public virtual IList<Set> CopiedInstances { get; set; }

    public Set(){
    }
}