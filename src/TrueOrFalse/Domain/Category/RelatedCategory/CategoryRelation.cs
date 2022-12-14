using System;
using System.Collections.Generic;
using System.Diagnostics;
using Seedworks.Lib.Persistence;

[DebuggerDisplay("{Category.Name}({CategoryId.Id}) {RelatedCategoryId.Name}({RelatedCategory.Id})")]
[Serializable]
public class CategoryRelation : DomainEntity
{
    public virtual Category Category { get; set; }

    public virtual Category RelatedCategory { get; set; }
}
