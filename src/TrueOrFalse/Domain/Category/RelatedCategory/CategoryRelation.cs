using System;
using System.Diagnostics;
using Seedworks.Lib.Persistence;

[DebuggerDisplay("{Category.Name}({Category.Id}) [{CategoryRelationType.ToString()}] {RelatedCategory.Name}({RelatedCategory.Id})")]
[Serializable]
public class CategoryRelation : DomainEntity
{
    public virtual Category Category { get; set; }

    public virtual Category RelatedCategory { get; set; }

    public virtual CategoryRelationType CategoryRelationType { get; set; }
}
